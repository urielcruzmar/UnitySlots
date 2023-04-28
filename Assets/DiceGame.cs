using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiceGame : MonoBehaviour
{
    private Sprite[] _diceSides;
    private SpriteRenderer _renderer;
    [SerializeField] private int inputValue;
    [SerializeField] private TMP_Text outputText;
    private int _totalSix;
    
    // Start is called before the first frame update
    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _diceSides = Resources.LoadAll<Sprite>("DiceSides");
        _totalSix = 0;
    }

    public IEnumerator RollDice()
    {
        int randomDiceSide = 0, finalSide = 0;
        inputValue = GameObject.FindGameObjectWithTag("UI").GetComponent<GameUI>().inputNumber;
        for (int i = 0; i < 20; i++)
        {
            randomDiceSide = Random.Range(0, 6);
            Debug.Log("DICES:"+_diceSides+"---"+randomDiceSide);
            _renderer.sprite = _diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.09f);
        }
        finalSide = randomDiceSide + 1;
        outputText.text = inputValue == finalSide ? $"Result: {finalSide} \nYou win!" : $"Result: {finalSide} \nYou lose!";
        Debug.Log("Total six: "+_totalSix);
    }
}
