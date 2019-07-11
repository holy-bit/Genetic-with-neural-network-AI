using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork
{
    public struct NetOutput
    {
        public int  id;
        public bool action;

        public NetOutput(int id_P, bool action_P) { id = id_P; action = action_P; }
    }

    // Start is called before the first frame update
    private static NeuralNetwork instance = null;

    // Game Instance Singleton
    public static NeuralNetwork Instance
    {
        get
        {
            return instance;
        }
    }
    public Layer inputLayer, hiddenLayer, outputLayer;

    public NeuralNetwork(int numberInputNodes, int numberHiddenNodes, int numberOutputNodes)
    {
        if(instance == null)
        instance = this;

        inputLayer  = new Layer(0, numberInputNodes, numberHiddenNodes);
        hiddenLayer = new Layer(numberInputNodes, numberHiddenNodes, numberOutputNodes);
        outputLayer = new Layer(numberHiddenNodes, numberOutputNodes, 0);

        inputLayer.Inicialize(numberInputNodes, null, hiddenLayer);
        hiddenLayer.Inicialize(numberHiddenNodes, inputLayer, outputLayer);
        outputLayer.Inicialize(numberOutputNodes, hiddenLayer, null);

        inputLayer.AssingRandomWeights();
        hiddenLayer.AssingRandomWeights();
    }

    public void SetInput(int i, float valor)
    {
        if (i >= 0 && i < inputLayer.numNodes)
        {
            inputLayer.neuronValues[i] = valor;
        }
    }

    public float GetOutput(int i)
    {
        if (i >= 0 && i < outputLayer.numNodes)
        {
            return outputLayer.neuronValues[i];
        }
        else
        {
            return -1;
        }
    }
    public void SetOutputDeseado(int i, float valor)
    {
        if (i >= 0 && i < outputLayer.numNodes)
        {
            outputLayer.desireValues[i] = valor;
        }
    }

    public void FeedForward()
    {
        inputLayer.CalculateNeuronValues();
        hiddenLayer.CalculateNeuronValues();
        outputLayer.CalculateNeuronValues();
    }

    public void BackPropagation()
    {
        outputLayer.CalculateErrors();
        hiddenLayer.CalculateErrors();

        hiddenLayer.FitWeights();
        inputLayer.FitWeights();
    }

    public NetOutput GetMaxOutputId()
    {
        NetOutput output = new NetOutput(-1,false);
        float max = float.MinValue;
        for (int i = 1; i < outputLayer.numNodes; i++)
        {
            if (outputLayer.neuronValues[i] > max)
            {
                max = outputLayer.neuronValues[i];
                output.id = i;
            }
            
        }
        if(outputLayer.neuronValues[0]>0.5)
        output.action = true;
        else output.action = false;
        return output;
    }

    public float CalculateError()
    {
        float error = 0;
        for (int i = 0; i < outputLayer.numNodes; i++)
        {
            error += Mathf.Pow(outputLayer.neuronValues[i] - outputLayer.desireValues[i], 2);
        }
        error /= outputLayer.numNodes;
        return error;
    }
}
