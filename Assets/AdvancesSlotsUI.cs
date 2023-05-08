using UnityEngine;
using UnityEngine.UI;

public class AdvancesSlotsUI : MonoBehaviour
{
    [SerializeField] private Button rollButton;
    private AdvancedSlotMachine _advancedSlotMachine;
    
    private void Start()
    {
        _advancedSlotMachine = GameObject.Find("SlotMachine").GetComponent<AdvancedSlotMachine>();
        rollButton.onClick.AddListener(() =>
        {
            _advancedSlotMachine.Turn();
        });
    }
}
