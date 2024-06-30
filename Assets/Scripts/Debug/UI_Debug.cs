using TMPro;
using UnityEngine;

public class UI_Debug : MonoBehaviour
{
    [SerializeField] LumberjackController NPC;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _numberOfStatesText;

    private void Update()
    {
        if(NPC._stateMachine.CurrentState() != null)
            _text.text = NPC._stateMachine.CurrentState().ToString();
        _numberOfStatesText.text = NPC._stateMachine.GetStatesAmount().ToString();
    }

    public void EmptyWater()
    {
        
    }
}
