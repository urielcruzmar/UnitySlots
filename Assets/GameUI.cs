using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Button rollButton;
    [SerializeField] private bool isNumber;

    public int inputNumber;
    public TMP_Text outputText;
    [SerializeField] private TMP_InputField inputField;
    private DiceGame _diceGame;
    
    // Start is called before the first frame update
    private void Start()
    {
        _diceGame = GameObject.Find("Dice").GetComponent<DiceGame>();
        rollButton.onClick.AddListener(() =>
        {
            isNumber = int.TryParse(inputField.text, out inputNumber);
            Debug.Log("Your bid is: " + inputNumber);
            if (isNumber && inputNumber is >= 1 and <= 6)
            {
                StartCoroutine(_diceGame.RollDice());
            }
            else
            {
                outputText.text = "Invalid input";
                Debug.Log("Input not a number");
            }
        });
    }
}
