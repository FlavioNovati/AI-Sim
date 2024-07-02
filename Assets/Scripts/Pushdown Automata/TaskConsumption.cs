using UnityEngine;

[System.Serializable]
public class TaskConsumption
{
    [SerializeField] public float HungerConsumption;
    [SerializeField] public float ThirstConsumption;

    public TaskConsumption(float hungerConsumption, float thirstConsumption)
    {
        HungerConsumption = hungerConsumption;
        ThirstConsumption = thirstConsumption;
    }
}
