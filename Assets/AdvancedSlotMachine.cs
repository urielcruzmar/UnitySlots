using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class AdvancedSlotMachine : MonoBehaviour
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

    // Weighted probability variables
    private struct WeightedProbability
    {
        public int Number;
        public int Weight;
    }
    private readonly List<WeightedProbability> _weightedDice = new List<WeightedProbability>();
    
    // Number to increase probability
    [SerializeField] private int increasedNumber = 6;
    // Percentage to increase the probability (0-100)
    [SerializeField] private int increasedProbability = 50; 
    
    private void Start()
    {
        // Renderer dices
        _diceSides = Resources.LoadAll<Sprite>($"DiceSides");
        // Add weights
        _weightedDice.Add(new WeightedProbability()
        {
            Number = increasedNumber,
            Weight = increasedProbability
        });
        var otherValuesProbability = (100 - increasedProbability) / 5;
        for (var i = 0; i < 6; i++)
        {
            _weightedDice.Add(new WeightedProbability()
            {
                Number = i,
                Weight = otherValuesProbability
            });
        }
    }

    private void FixedUpdate()
    {
        // Check if player spins
        if (!_startSpin) return;
        _elapsedTime += Time.deltaTime;
        var randomSpinResult = Random.Range(0, SymbolNumber);
        // Spin dice 1
        if (!_dice1Spin)
        {
            if (!_dice1Spinning)
            {
                StartCoroutine(RollDiceUI(dice1));
                _dice1Spinning = true;
            }
            if (!(_elapsedTime >= spinDuration)) return;
            var randomWeighted = PickNumber();
            _resultDice1 = randomWeighted;
            _dice1Spin = true;
            dice1.sprite = _diceSides[_resultDice1-1];
            _elapsedTime = 0;
        }
        // Spin dice 2
        else if (!_dice2Spin)
        {
            if (!_dice2Spinning)
            {
                StartCoroutine(RollDiceUI(dice2));
                _dice2Spinning = true;
            }
            if (!(_elapsedTime >= spinDuration)) return;
            var randomWeighted = PickNumber();
            _resultDice2 = randomWeighted;
            _dice2Spin = true;
            dice2.sprite = _diceSides[_resultDice2-1];
            _elapsedTime = 0;
        }
        // Spin dice 3
        else if (!_dice3Spin)
        {
            if (!_dice3Spinning)
            {
                StartCoroutine(RollDiceUI(dice3));
                _dice3Spinning = true;
            }
            if (!(_elapsedTime >= spinDuration)) return;
            // Trick almost win
            if (_resultDice1 == _resultDice2 && randomSpinResult != _resultDice1)
            {
                randomSpinResult = _resultDice1 - 1;
                if (randomSpinResult < _resultDice1)
                {
                    randomSpinResult = _resultDice1 - 1;
                }
                if (randomSpinResult > _resultDice1)
                {
                    randomSpinResult = _resultDice1 + 1;
                }
                if (randomSpinResult < 0)
                {
                    randomSpinResult = 0;
                }
                if (randomSpinResult > 9)
                {
                    randomSpinResult = 9;
                }
                _resultDice3 = randomSpinResult;
            }
            else
            {
                var randomWeighted = PickNumber();
                _resultDice3 = randomWeighted;
            }
            _elapsedTime = 0;
            // All dices done
            _startSpin = false;
            _dice1Spin = false;
            _dice2Spin = false;
            _dice3Spin = false;
            dice3.sprite = _diceSides[_resultDice3-1];
            _dice1Spinning = false; 
            _dice2Spinning = false; 
            _dice3Spinning = false;
            CheckResult();
        }
    }
    
    // Show result and manage credits
    private void CheckResult()
    {
        if (_resultDice1 == _resultDice2 && _resultDice2 == _resultDice3)
        {
            bidResult.text = "JACKPOT!";
            _credit += 50 * _bidAmount;
        }
        else if (_resultDice1 == 0 && _resultDice3 == 0)
        {
            bidResult.text = "YOU WIN " + _bidAmount / 2;
            _credit -= _bidAmount / 2;
        }
        else if (_resultDice1 == _resultDice2)
        {
            bidResult.text = "YOU ALMOST GOT IT";
            _credit -= _bidAmount;
        }
        else if (_resultDice1 == _resultDice3)
        {
            bidResult.text = "YOU WIN " + _bidAmount * 2;
            _credit += _bidAmount * 2;
        }
        else
        {
            bidResult.text = "YOU LOSE!";
            _credit -= _bidAmount;
        }
        Debug.Log("1: "+_resultDice1);
        Debug.Log("2: "+_resultDice2);
        Debug.Log("3: "+_resultDice3);
    }

    // Pick a random weighted number
    private int PickNumber()
    {
        var totalWeights = _weightedDice.Sum(symbol => symbol.Weight);
        var randomNumber = Random.Range(0, totalWeights);
        var i = 0;
        while (randomNumber >= 0)
        {
            var candidate = _weightedDice[i];
            randomNumber -= candidate.Weight;
            if (randomNumber <= 0)
            {
                return candidate.Number;
            }
            i++;
        }
        throw new Exception("Something is wrong in PickNumber()");
    }

    // Start spin
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

    // UI Update
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
