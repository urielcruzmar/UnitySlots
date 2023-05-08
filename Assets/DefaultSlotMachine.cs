using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DefaultSlotMachine : MonoBehaviour
{
    // Dice variables
    private Sprite[] _diceSides;
    private const int SymbolNumber = 6;

    // UI variables & Spin duration
    [SerializeField] private float spinDuration = 2.0f;
    [SerializeField] private SpriteRenderer dice1;
    [SerializeField] private SpriteRenderer dice2;
    [SerializeField] private SpriteRenderer dice3;
    [SerializeField] private TextMeshProUGUI bidResult;
    [SerializeField] private TextMeshProUGUI totalCredit;
    [SerializeField] private TMP_InputField inputBid;

    // Game variables
    private int _resultDice1;
    private int _resultDice2;
    private int _resultDice3;
    private bool _startSpin;
    private bool _dice1Spin;
    private bool _dice2Spin;
    private bool _dice3Spin;
    private bool _dice1Spinning;
    private bool _dice2Spinning;
    private bool _dice3Spinning;
    private float _elapsedTime;
    private int _bidAmount;
    private int _credit = 1000;

    private void Start()
    {
        // Renderer dices
        _diceSides = Resources.LoadAll<Sprite>($"DiceSides");
    }

    private void FixedUpdate()
    {
        if (!_startSpin) return;
        
        _elapsedTime += Time.deltaTime;
        int randomSpinResult;
        // RAIL 1
        if (!_dice1Spin)
        {
            if (!_dice1Spinning)
            {
                StartCoroutine(RollDiceUI(dice1));
                _dice1Spinning = true;
            }
            if (!(_elapsedTime >= spinDuration)) return;
            randomSpinResult = Random.Range(0, SymbolNumber);
            _resultDice1 = randomSpinResult;
            _dice1Spin = true;
            dice1.sprite = _diceSides[_resultDice1-1];
            _elapsedTime = 0;
        }
        // RAIL 2
        else if (!_dice2Spin)
        {
            if (!_dice2Spinning)
            {
                StartCoroutine(RollDiceUI(dice2));
                _dice2Spinning = true;
            }
            if (!(_elapsedTime >= spinDuration)) return;
            randomSpinResult = Random.Range(0, SymbolNumber);
            _resultDice2 = randomSpinResult;
            _dice2Spin = true;
            dice2.sprite = _diceSides[_resultDice2-1];
            _elapsedTime = 0;
        }
        // RAIL 3
        else if (!_dice3Spin)
        {
            if (!_dice3Spinning)
            {
                StartCoroutine(RollDiceUI(dice3));
                _dice3Spinning = true;
            }
            if (!(_elapsedTime >= spinDuration)) return;
            randomSpinResult = Random.Range(0, SymbolNumber);
            _resultDice3 = randomSpinResult;
            _dice3Spin = true;
            dice3.sprite = _diceSides[_resultDice3-1];
            _elapsedTime = 0;
            // ALL RAILS DONE
            _startSpin = false;
            _dice1Spin = false;
            _dice2Spin = false;
            _dice3Spin = false;
            CheckResult();
        }
    }
    
    public void Turn()
    {
        if (_bidAmount > 0)
        {
            _startSpin = true;
        }
        else
        {
            bidResult.text = "Bid a valid amount!";
        }
    }

    private void OnGUI()
    {
        try
        {
            _bidAmount = int.Parse(inputBid.text);
        }
        catch
        {
            _bidAmount = 0;
        }
        totalCredit.text = _credit.ToString();
    }

    private void CheckResult()
    {
        if (_resultDice1 == _resultDice2 && _resultDice2 == _resultDice3)
        {
            bidResult.text = "YOU WIN!";
            _credit += 50 * _bidAmount;
        }
        else
        {
            bidResult.text = "YOU LOSE";
            _credit -= _bidAmount;
        }
        Debug.Log("NORMAL");
    }
    
    // Roll dice (UI only)
    private IEnumerator RollDiceUI(SpriteRenderer dice)
    {
        for (var i = 0; i < 20; i++)
        {
            dice.sprite = _diceSides[Random.Range(0, 6)];
            yield return new WaitForSeconds(0.09f);
        }
    }
}
