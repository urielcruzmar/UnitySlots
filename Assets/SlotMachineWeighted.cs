using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class SlotMachineWeighted : MonoBehaviour
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

    [SerializeField]
    public struct WeightedProbability
    {
        public int number;
        public int weight;
    }

    private List<WeightedProbability> _weightedRail = new List<WeightedProbability>();
    private int zeroProbability = 50;

    private int resultRail1 = 0;
    private int resultRail2 = 0;
    private int resultRail3 = 0;

    private float elapsedTime = 0.0f;
    
    private void Start()
    {
        _weightedRail.Add(new WeightedProbability()
        {
            number = 0,
            weight = zeroProbability
        });
        var otherValuesProbability = (100 - zeroProbability) / 9;
        for (var i = 0; i < 10; i++)
        {
            _weightedRail.Add(new WeightedProbability()
            {
                number = i,
                weight = otherValuesProbability
            });
        }
    }
    
    private void CheckResult()
    {
        if (resultRail1 == resultRail2 && resultRail2 == resultRail3)
        {
            bidResult.text = "JACKPOT!";
            credit += 50 * bidAmount;
        }
        else if (resultRail1 == 0 && resultRail3 == 0)
        {
            bidResult.text = "YOU WIN " + bidAmount / 2;
            credit -= bidAmount / 2;
        }
        else if (resultRail1 == resultRail2)
        {
            bidResult.text = "YOU ALMOST GOT IT";
            credit -= bidAmount;
        }
        else if (resultRail1 == resultRail3)
        {
            bidResult.text = "YOU WIN " + bidAmount * 2;
            credit += bidAmount * 2;
        }
        else
        {
            bidResult.text = "YOU LOSE!";
            credit -= bidAmount;
        }
    }

    private int PickNumber()
    {
        var totalWeights = _weightedRail.Sum(symbol => symbol.weight);
        var randomNumber = Random.Range(0, totalWeights);
        var i = 0;
        while (randomNumber >= 0)
        {
            var candidate = _weightedRail[i];
            randomNumber -= candidate.weight;
            if (randomNumber <= 0)
            {
                return candidate.number;
            }
            i++;
        }
        throw new Exception("Something is wrong in PickNumber()");
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
            var randomWeighted = PickNumber();
            rail1.text = randomWeighted.ToString();
            resultRail1 = randomWeighted;
            _rail1Spin = true;
            elapsedTime = 0;
        }
        // RAIL 2
        else if (!_rail2Spin)
        {
            rail2.text = randomSpinResult.ToString();
            if (!(elapsedTime >= spinDuration)) return;
            var randomWeighted = PickNumber();
            rail2.text = randomWeighted.ToString();
            resultRail2 = randomWeighted;
            _rail2Spin = true;
            elapsedTime = 0;
        }
        // RAIL 3
        else if (!_rail3Spin)
        {
            rail3.text = randomSpinResult.ToString();
            if (!(elapsedTime >= spinDuration)) return;
            // Trick
            if (resultRail1 == resultRail2 && randomSpinResult != resultRail1)
            {
                randomSpinResult = resultRail1 - 1;
                if (randomSpinResult < resultRail1)
                {
                    randomSpinResult = resultRail1 - 1;
                }
                if (randomSpinResult > resultRail1)
                {
                    randomSpinResult = resultRail1 + 1;
                }
                if (randomSpinResult < 0)
                {
                    randomSpinResult = 0;
                }
                if (randomSpinResult > 9)
                {
                    randomSpinResult = 9;
                }
                rail3.text = randomSpinResult.ToString();
                resultRail3 = randomSpinResult;
            }
            else
            {
                var randomWeighted = PickNumber();
                rail3.text = randomWeighted.ToString();
                resultRail3 = randomWeighted;
            }
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
}
