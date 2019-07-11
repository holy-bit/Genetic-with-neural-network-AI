using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NeuralNetworkRabbits : MonoBehaviour
{
    private NeuralNetwork net;
    public int numInput, numHidden, numOutput;

    private static float[,] entrenamiento =
    {
        // INPUT: distancia_planta, distancia_enemigo, distancia_presa, Relacion_de_tamaño, enegia, primavera, verano, otoño, invierno.
        // OUTPUT: Action, planta, depredador, presa.

        {0.1f, 1   , 1   , 0, 0.1f,  1, 0, 0, 0,        1, 1, 0, 0}, 

        {1f, 0.8f, 1   , 1, 0.6f,  1, 0, 0, 0,        0, 0, 1, 0}, 
        {1f, 0.8f, 1   , 1, 0.6f,  0, 1, 0, 0,        0, 0, 1, 0}, 
        {1f, 0.8f, 1   , 1, 0.6f,  0, 0, 1, 0,        0, 0, 1, 0}, 
        {1f, 0.8f, 1   , 1, 0.6f,  0, 0, 0, 1,        0, 0, 1, 0}, 
        {1f, 1   , 0.5f, 0, 0.8f,  1, 0, 0, 0,        1, 0, 0, 1}, 
        {1f, 1   , 0.5f, 0, 0.8f,  0, 1, 0, 0,        1, 0, 0, 1}, 
        {1f, 1   , 0.5f, 0, 0.8f,  0, 0, 1, 0,        1, 0, 0, 1},
        {1f, 1   , 0.5f, 0, 0.8f,  0, 0, 0, 1,        1, 0, 0, 1}, 
           
        {0.5f, 1   , 0.5f, 0, 0.8f,  1, 0, 0, 0,      1, 0, 0, 1}, 
        {0.5f, 1   , 0.5f, 0, 0.8f,  0, 1, 0, 0,      1, 0, 0, 1},
        {0.5f, 1   , 0.5f, 0, 0.8f,  0, 0, 1, 0,      1, 0, 0, 1}, 
        {0.5f, 1   , 0.5f, 0, 0.8f,  0, 0, 0, 1,      1, 0, 0, 1}, 
   };

    public static NeuralNetworkRabbits instance;

    // Start is called before the first frame update
    void Start()
    {
        numInput = 9;
        numHidden = 6;
        numOutput = 4;
        instance = this;
        net = new NeuralNetwork(numInput, numHidden, numOutput);
        NetworkTraining();
    }
    void NetworkTraining()
    {
        float error = 1;
        int epoch = 0;
        while ((error > 0.05f) && (epoch < 50000))
        {
            error = 0;
            epoch++;
            for (int i = 0; i < entrenamiento.GetLength(0); i++)
            {
                for (int j = 0; j < numInput; j++)
                {
                    net.SetInput(j, entrenamiento[i, j]);
                }
                for (int j = numInput; j < entrenamiento.GetLength(1); j++)
                {
                    net.SetOutputDeseado(j - numInput, entrenamiento[i, j]);
                }
                net.FeedForward();
                error += net.CalculateError();
                net.BackPropagation();
            }
            error /= entrenamiento.GetLength(0);
        }
    }

    public void NetworkRetraining(float[] input, float[] output)
    {
        float error = 1;
        int epoch = 0;
        while ((error > 0.1f) && (epoch < 1000))
        {
            epoch++;
            for (int j = 0; j < numInput; j++)
            {
                net.SetInput(j, input[j]);
            }
            for (int j = 0; j < numOutput; j++)
            {
                net.SetOutputDeseado(j, output[j]);
            }
            net.FeedForward();
            error = net.CalculateError();
            net.BackPropagation();
        }
    }

    public NeuralNetwork.NetOutput getAction(float[] input)
    {
        for (int j = 0; j < numInput; j++)
        {
            net.SetInput(j, input[j]);
        }
        net.FeedForward();
        return new NeuralNetwork.NetOutput(net.GetMaxOutputId().id, net.GetMaxOutputId().action);
    }
}
