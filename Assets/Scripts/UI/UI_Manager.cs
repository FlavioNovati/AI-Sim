using PushdownAutomata;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private TMP_Text _statesTextField;
    [Header("Day UI")]
    [SerializeField] private Image _clockImage;
    [SerializeField, Range(0f, 360f)] private float _startingRotation = 0f;
    [SerializeField, Range(0f, 360f)] private float _finalRotation = 360f;
    [Header("Necessity UI")]
    [SerializeField] private Image _hungerBar;
    [SerializeField] private Image _thirstBar;
    [SerializeField] private Color _hungerCriticalColor;
    [SerializeField] private Color _thirstCriticalColor;
    
    private IEntity _entity => _lumberjack;
    private LumberjackController _lumberjack;
    private NecessityVisualizer _thirstVisualizer;
    private NecessityVisualizer _hungerVisualizer;
    
    private void Awake()
    {
        _lumberjack = FindObjectOfType<LumberjackController>();
        _lumberjack.OnSetup += UpdateUIStates;
        
        _hungerVisualizer = new NecessityVisualizer(_entity, _hungerBar, _hungerCriticalColor, true);
        _thirstVisualizer = new NecessityVisualizer(_entity, _thirstBar, _hungerCriticalColor, false);
    }

    private void Update()
    {
        _hungerVisualizer.Update();
        _thirstVisualizer.Update();

        UpdateUIStates();

        float clockRotation = Mathf.Lerp(_startingRotation, _finalRotation, DayManager.Instance.DayProgress);
        _clockImage.rectTransform.eulerAngles = new Vector3(0f, 0f, clockRotation);
    }

    private void UpdateUIStates()
    {
        string statesString = "";
        //Add options
        List<PDA_State> states = _lumberjack.StateMachine.GetStates();
        for(int i=0; i < states.Count; i++)
            statesString += states[i].ToString()+"\n";
        //Update text
        _statesTextField.text = statesString;
    }
}
