using TMPro;
using UnityEngine;

public class UI_Debug : MonoBehaviour
{
    [SerializeField] LumberjackController NPC;
    [SerializeField] private TMP_Text m_Text;
    [SerializeField] private TMP_Text m_NumberOfStatesText;

    private void Update()
    {
        m_Text.text = NPC.m_StateMachine.CurrentState().ToString();
        m_NumberOfStatesText.text = NPC.m_StateMachine.GetStatesAmount().ToString();
    }

    public void EmptyWater()
    {
        NPC.Drink();
    }
}
