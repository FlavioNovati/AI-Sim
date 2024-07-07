using System;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static Action OnDayFinished = () => { };
    public static Action OnDayStarted = () => { };

    public static DayManager Instance;

    [SerializeField] private float _dayDuration = 60f;
    [SerializeField] private float _nightDuration = 35f;

    private float _time = 0f;
    public float DayProgress { get; private set; } = 0f;
    private bool _allowDayFinishedCall = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Update()
    {
        //Increse Time
        _time += Time.deltaTime;

        //Day Ended
        if(_time > _dayDuration && _allowDayFinishedCall)
        {
            OnDayFinished?.Invoke();
            _allowDayFinishedCall = false;
        }

        //Night Ended
        if (_time > _dayDuration + _nightDuration)
        {
            _time = 0f;
            OnDayStarted?.Invoke();
            _allowDayFinishedCall = true;
        }

        //Calculate day progress
        DayProgress = _time / (_dayDuration + _nightDuration);
    }
}
