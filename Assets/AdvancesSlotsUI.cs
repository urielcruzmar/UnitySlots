using UnityEngine;
using UnityEngine.UI;

public class AdvancesSlotsUI : MonoBehaviour
{
    [SerializeField] private Button rollButton;
    [SerializeField] private Toggle tricked;
    private AdvancedSlotMachine _advancedSlotMachine;
    private DefaultSlotMachine _defaultSlotMachine;
    
    private void Start()
    {
        _advancedSlotMachine = GameObject.Find("SlotMachine").GetComponent<AdvancedSlotMachine>();
        _defaultSlotMachine = GameObject.Find("SlotMachine").GetComponent<DefaultSlotMachine>();
        rollButton.onClick.AddListener(() =>
        {
            if (tricked.isOn)
            {
                _advancedSlotMachine.Turn();
            }
            else
            {
                _defaultSlotMachine.Turn();
            }
        });
    }
}
