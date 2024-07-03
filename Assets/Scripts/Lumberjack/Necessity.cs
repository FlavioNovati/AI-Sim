using UnityEngine;

[System.Serializable]
public class Necessity
{
    public delegate void CritLevel();
    public event CritLevel OnCritical;

    [SerializeField] public float MaxValue = 20f;
    [SerializeField] public float StartingValue = 15f;
    [SerializeField] public float CriticalLevelThreshold = 3f;
    
    public float Value { get; private set; } = 10f;
    private bool _allowCritEvent = false;

    public Necessity()
    {
        Value = StartingValue;
        _allowCritEvent = true;
    }

    public void Decrease(float amount)
    {
        Value -= amount;
        //Clamp hunger
        Value = Mathf.Clamp(Value, 0f, MaxValue);
        //Value has reached crit level
        if (Value <= CriticalLevelThreshold && _allowCritEvent)
        {
            OnCritical?.Invoke();
            _allowCritEvent = false;
        }
    }

    public void Increase(float amount)
    {
        Value += amount;
        //Allow crit event
        if(Value > CriticalLevelThreshold)
            _allowCritEvent = true;
        //Clamp value
        Value = Mathf.Clamp(Value, 0f, MaxValue);
    }

    public bool IsCritical()
    {
        return Value < CriticalLevelThreshold;
    }
}
