using UnityEngine;

[System.Serializable]
public class LumberjackStats
{
    [SerializeField] public float MaxHunger = 20f;
    [SerializeField] public float MaxThirst = 20f;
    public float HungerAmount { get; private set; } = 10f;
    public float ThirstAmount { get; private set; } = 15f;

    public LumberjackStats(float hungerAmount, float thirstAmount)
    {
        HungerAmount = hungerAmount;
        ThirstAmount = thirstAmount;
    }

    public void ChangeHunger(float amount)
    {
        HungerAmount += amount;
    }

    public void ChangeThirst(float amount)
    {
        ThirstAmount += amount;
    }
}
