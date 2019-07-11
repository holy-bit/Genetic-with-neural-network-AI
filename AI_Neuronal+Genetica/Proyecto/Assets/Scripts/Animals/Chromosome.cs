using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chromosome
{
    public List<Gene> genes { get; }

    public Chromosome(List<Gene> newGenes)
    {
        genes = newGenes;
    }

    public List<Gene> Recombination(List<Gene> otherGenes, float mutationFactor)
    {
        List<Gene> newGenes;

        if (Random.Range(0, 10) <= 4)
            newGenes = genes;
        else
            newGenes = otherGenes;

        float finalMutation = Random.Range(-mutationFactor, mutationFactor);
        
        for(int i = 0; i < newGenes.Count; ++i)
        {
            newGenes[i].value += finalMutation;
            newGenes[i].value = Mathf.Clamp(newGenes[i].value, newGenes[i].minValue, newGenes[i].maxValue);
        }
        
        return newGenes;

    }
}
