using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SlotMachine : MonoBehaviour
{
    [SerializeField] private float spinDuration = 2.0f;
    [SerializeField] private int symbolNumber = 10;

    [SerializeField] private TextMeshProUGUI rail1;
    [SerializeField] private TextMeshProUGUI rail2;
    [SerializeField] private TextMeshProUGUI rail3;
    [SerializeField] private TextMeshProUGUI bidResult;
    [SerializeField] private TextMeshProUGUI totalCredit;
    [SerializeField] private TMP_InputField inputBid;

    private bool _startSpin = false;
    private bool _rail1Spin = false;
    private bool _rail2Spin = false;
    private bool _rail3Spin = false;

    private int bidAmount;
    private int credit = 1000;

    private int resultRail1 = 0;
    private int resultRail2 = 0;
    private int resultRail3 = 0;

    private float elapsedTime = 0.0f;

    public void Turn()
    {
        if (bidAmount > 0)
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
            bidAmount = int.Parse(inputBid.text);
        }
        catch
        {
            bidAmount = 0;
        }
        totalCredit.text = credit.ToString();
    }

    private void CheckResult()
    {
        if (resultRail1 == resultRail2 && resultRail2 == resultRail3)
        {
            bidResult.text = "YOU WIN!";
            credit += 50 * bidAmount;
        }
        else
        {
            bidResult.text = "YOU LOSE";
            credit -= bidAmount;
        }
    }

    private void FixedUpdate()
    {
        if (!_startSpin) return;
        
        elapsedTime += Time.deltaTime;
        var randomSpinResult = Random.Range(0, symbolNumber);
        // RAIL 1
        if (!_rail1Spin)
        {
            rail1.text = randomSpinResult.ToString();
            if (!(elapsedTime >= spinDuration)) return;
            resultRail1 = randomSpinResult;
            _rail1Spin = true;
            elapsedTime = 0;
        }
        // RAIL 2
        else if (!_rail2Spin)
        {
            rail2.text = randomSpinResult.ToString();
            if (!(elapsedTime >= spinDuration)) return;
            resultRail2 = randomSpinResult;
            _rail2Spin = true;
            elapsedTime = 0;
        }
        // RAIL 3
        else if (!_rail3Spin)
        {
            rail3.text = randomSpinResult.ToString();
            if (!(elapsedTime >= spinDuration)) return;
            resultRail3 = randomSpinResult;
            _rail3Spin = true;
            elapsedTime = 0;
            // ALL RAILS DONE
            _startSpin = false;
            _rail1Spin = false;
            _rail2Spin = false;
            _rail3Spin = false;
            CheckResult();
        }
    }
}
