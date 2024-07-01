using UnityEngine;

[System.Serializable]
public class Necessity
{
    public delegate void CritLevel();
    public event CritLevel OnCrit;

    [SerializeField] public float MaxValue = 20f;
    [SerializeField] public float StartingValue = 15f;
    [SerializeField] public float CritLevelThreshold = 3f;
    
    public float Value { get; private set; } = 10f;
    private bool _allowCritEvent = true;

    public Necessity()
    {
        Value = StartingValue;
    }

    public void Decrease(float amount)
    {
        Value -= amount;
        //Clamp hunger
        Value = Mathf.Clamp(Value, 0f, MaxValue);
        //Hunger has reached crit level
        if (Value <= CritLevelThreshold && _allowCritEvent)
        {
            OnCrit?.Invoke();
            _allowCritEvent = false;
        }
    }

    public void Increase(float amount)
    {
        Value += amount;
        //Allow crit event
        if(Value > CritLevelThreshold)
            _allowCritEvent = true;
        //Clamp Hunger
        Value = Mathf.Clamp(Value, 0f, MaxValue);
    }
}
