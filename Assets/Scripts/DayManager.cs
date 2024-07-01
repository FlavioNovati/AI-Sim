using System;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static Action OnDayFinished = () => { };
    public static Action OnDayStarted = () => { };

    [SerializeField] private float _dayDuration = 60f;
    [SerializeField] private float _nightDuration = 35f;
    [SerializeField] private float _time = 0f;

    private bool _nightCalled = false;

    private void Update()
    {
        _time += Time.deltaTime;

        if(_time > _dayDuration && !_nightCalled)
        {
            OnDayFinished?.Invoke();
            _nightCalled = true;
        }
        if(_time > _dayDuration + _nightDuration)
        {
            _time = 0f;
            OnDayStarted?.Invoke();
            _nightCalled = false;
        }
    }
}
