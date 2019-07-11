using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gene
{

    public float value { get; set;}
    public float minValue { get; }
    public float maxValue { get; }
    public float energyConsumptionFactor { get; } //Per second

    public Gene(float value, float minValue, float maxValue, float energyConsumptionFactor)
    {
        this.value = value;
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.energyConsumptionFactor = energyConsumptionFactor;
    }
    public Gene(float value, float minValue, float maxValue)
    {
        this.value = value;
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.energyConsumptionFactor = 0.01f;
    }

    public Gene(float minValue, float maxValue)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.value = Random.Range(minValue, maxValue);
        this.energyConsumptionFactor = 0.01f;
    }
}
