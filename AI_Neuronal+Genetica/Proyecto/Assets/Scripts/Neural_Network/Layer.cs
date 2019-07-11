using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer
{
    public int numNodes, numChildNode, numParentNodes;
    public float[,] weights;
    public float[,] weightsIncrement;
    public float[] neuronValues;
    public float[] desireValues;
    public float[] errors;
    public float[] biasValues;
    public float[] biasweights;

    public Layer parentLayer, childLayer;

    public Layer(int numberParentNodes, int numberNodos, int numberChildNodes)
    {
        numNodes = numberNodos;
        numChildNode = numberChildNodes;
        numParentNodes = numberParentNodes;
        childLayer = null;
        parentLayer = null;
    }

    public void Inicialize(int numberNodes, Layer parent, Layer child)
    {
        numNodes = numberNodes;

        neuronValues = new float[numNodes];
        desireValues = new float[numNodes];
        errors = new float[numNodes];

        if (parent != null)
        {
            parentLayer = parent;
            numParentNodes = parent.numNodes;
        }
        else
        {
            parentLayer = null;
            numParentNodes = 0;
        }

        if (child != null)
        {
            childLayer  = child;
            numChildNode = child.numNodes;
            weights = new float[numNodes, numChildNode];
            weightsIncrement = new float[numNodes, numChildNode];
            biasValues = new float[numChildNode];
            biasweights = new float[numChildNode];

            for (int i = 0; i < numNodes; i++)
            {
                for (int j = 0; j < numChildNode; j++)
                {
                    weights[i, j] = 0;
                    weightsIncrement[i, j] = 0;
                }
            }
            for (int j = 0; j < numChildNode; j++)
            {
                biasValues[j] = -1;
                biasweights[j] = 0;
            }
        }
        else
        {
            childLayer = null;
            numChildNode = 0;
            weights = null;
            weightsIncrement = null;
            biasValues = null;
            biasweights = null;
        }

        for (int i = 0; i < numNodes; i++)
        {
            neuronValues[i] = 0;
            desireValues[i] = 0;
            errors[i] = 0;
        }
     
    }

    public void CalculateNeuronValues()
    {
        if (parentLayer != null)
        {
            for (int j = 0; j < numNodes; j++)
            {
                float x = 0;
                for (int i = 0; i < numParentNodes; i++)
                {
                    x += parentLayer.neuronValues[i] * parentLayer.weights[i, j];
                }
                x += parentLayer.biasValues[j] * parentLayer.biasweights[j];

                if (childLayer == null && Const.OUTPUT_LINEAL)
                {
                    neuronValues[j] = x;
                }
                else
                {
                    neuronValues[j] = 1.0f / (1 + Mathf.Exp(-x));
                }
            }
        }
    }

    public void AssingRandomWeights()
    {
        if (childLayer != null)
        {
            for (int i = 0; i < numNodes; i++)
            {
                for (int j = 0; j < numChildNode; j++)
                {
                    weights[i, j] = Random.Range(-1f, 1f);
                }
            }
            for (int j = 0; j < numChildNode; j++)
            {
                biasweights[j] = Random.Range(-1f, 1f);
            }
        }
    }

    public void FitWeights()
    {
        if (childLayer != null)
        {
            for (int i = 0; i < numNodes; i++)
            {
                for (int j = 0; j < numChildNode; j++)
                {
                    float dw = Const.RATIO_APRENDIZAJE * childLayer.errors[j] * neuronValues[j]; // Revisar: Valores de j o de i.
                    if (Const.USO_INERCIA)
                    {
                        weights[i, j] += dw + Const.RATIO_INERCIA * weightsIncrement[i, j];
                        weightsIncrement[i, j] = dw;
                    }
                    else
                    {
                        weights[i, j] += dw;
                    }
                }
            }
            for (int j = 0; j < numChildNode; j++)
            {
                float dw = Const.RATIO_APRENDIZAJE * childLayer.errors[j] * biasValues[j];
                biasweights[j] += dw;
            }
        }
    }

    public void CalculateErrors()
    {
        if (childLayer == null)
        {
            for (int i = 0; i < numNodes; i++)
            {
                errors[i] = (desireValues[i] - neuronValues[i]) * neuronValues[i] * (1 - neuronValues[i]);
            }
        }
        else if (parentLayer != null)
        {
            for (int i = 0; i < numNodes; i++)
            {
                float suma = 0;
                for (int j = 0; j < numChildNode; j++)
                {
                    suma += childLayer.errors[j] * weights[i, j];
                }
                errors[i] = suma * neuronValues[i] * (1 - neuronValues[i]);
            }
        }
    }
}
