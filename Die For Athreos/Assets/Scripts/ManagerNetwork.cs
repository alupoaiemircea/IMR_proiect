using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using System.Globalization;

public struct Line
{
    public float x1;
    public float y1;
    public float z1;

    public float x2;
    public float y2;
    public float z2;

    public float health;
    public float enemyAttack;
    public float output;

}
public class ManagerNetwork : MonoBehaviour
{
    NeuralNetwork net;
    int[] layers = new int[3]{ 8, 5, 1 };
    string[] activation = new string[2] { "sigmoid", "sigmoid" };
    float multiplier = 200;
    float multiplierHealth = 20;
    float multiplierOutput = 10;
    private Line[] ReadFile(string path)
    {
        string[] lines = File.ReadAllLines(path);
        Line[] data = new Line[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            Line line = new Line();
            string[] split = lines[i].Split(" ");
            line.x1 = float.Parse(split[0])/multiplier;
            line.y1 = float.Parse(split[1])/multiplier;
            line.z1 = float.Parse(split[2])/multiplier;

            line.x2 = float.Parse(split[3])/multiplier;
            line.y2 = float.Parse(split[4])/multiplier;
            line.z2 = float.Parse(split[5])/multiplier;

            line.health = float.Parse(split[6])/multiplierHealth;
            line.enemyAttack = float.Parse(split[7]);
            line.output = float.Parse(split[8])/multiplierOutput;
            data[i] = line;
        }
        return data;
    }
    void Start()
    {
        this.net = new NeuralNetwork(layers, activation);
        Line[] trainigData = ReadFile("warrior_dataset.txt");
        Line[] testingData = ReadFile("warrior_test_set.txt");
        
        for (int i = 0; i < 20000; i++)
        {

            //net.BackPropagate(new float[] { 115.7321f, 1.0000f, 371.0684f, 116.9508f, 0.0781f, 369.6854f, 6.0f, 1.0f }, new float[] { 0 });
            //net.BackPropagate(new float[] { 115.7321f, 1.0000f, 371.0684f, 116.9508f, 0.0781f, 369.6854f, 6.0f, 1.0f }, new float[] { 0 });
            //net.BackPropagate(new float[] { 115.7321f, 1.0000f, 371.0684f, 116.9508f, 0.0781f, 369.6854f, 6.0f, 1.0f }, new float[] { 0 });
            //net.BackPropagate(new float[] { 115.7321f, 1.0000f, 371.0684f, 116.9508f, 0.0781f, 369.6854f, 6.0f, 1.0f }, new float[] { 0 });
            //net.BackPropagate(new float[] { 115.7321f, 1.0000f, 371.0684f, 116.9508f, 0.0781f, 369.6854f, 6.0f, 1.0f }, new float[] { 0 });
            //net.BackPropagate(new float[] { 115.7321f, 1.0000f, 371.0684f, 116.9508f, 0.0781f, 369.6854f, 6.0f, 1.0f }, new float[] { 0 });
            for (int j = 0; j < trainigData.Length; j++)
            {


                net.BackPropagate(new float[] {trainigData[j].x1, trainigData[j].y1, trainigData[j].z1,
                trainigData[j].x2,trainigData[j].y2,trainigData[j].z2,trainigData[j].health,trainigData[j].enemyAttack},
                new float[] { trainigData[j].output });
            }
        }
        //net.Save("network_brain.txt");
        //net.Load("network_brain.txt");
        for (int j = 0; j < testingData.Length; j++)
        {
            Debug.Log(net.FeedForward(new float[] {testingData[j].x1, testingData[j].y1,testingData[j].z1,
                testingData[j].x2,testingData[j].y2,testingData[j].z2,testingData[j].health,testingData[j].enemyAttack})[0]);
        }
        print("cost: "+net.cost);

        //UnityEngine.Debug.Log(net.FeedForward(new float[] { 2.0006f, 0.0789f, 0.0673f, 2.0006f, 0.0789f, 0.0673f, 2.0006f, 0.0789f })[0]);
        //UnityEngine.Debug.Log(net.FeedForward(new float[] { 2.0006f, 0.0789f, 0.0673f, 2.0006f, 0.0789f, 0.0673f, 2.0006f, 0.0789f })[0]);
        //UnityEngine.Debug.Log(net.FeedForward(new float[] { 2.0006f, 0.0789f, 0.0673f, 2.0006f, 0.0789f, 0.0673f, 2.0006f, 0.0789f })[0]);
        //UnityEngine.Debug.Log(net.FeedForward(new float[] { 2.0006f, 0.0789f, 0.0673f, 2.0006f, 0.0789f, 0.0673f, 2.0006f, 0.0789f })[0]);
        //UnityEngine.Debug.Log(net.FeedForward(new float[] { 2.0006f, 0.0789f, 0.0673f, 2.0006f, 0.0789f, 0.0673f, 2.0006f, 0.0789f })[0]);
        //UnityEngine.Debug.Log(net.FeedForward(new float[] { 2.0006f, 0.0789f, 0.0673f, 2.0006f, 0.0789f, 0.0673f, 2.0006f, 0.0789f })[0]);
        //UnityEngine.Debug.Log(net.FeedForward(new float[] { 2.0006f, 0.0789f, 0.0673f, 2.0006f, 0.0789f, 0.0673f, 2.0006f, 0.0789f })[0]);
        //UnityEngine.Debug.Log(net.FeedForward(new float[] { 1, 1, 1 })[0]);
        //We want the gate to simulate 3 input or gate (A or B or C)
        // 0 0 0    => 0
        // 1 0 0    => 1
        // 0 1 0    => 1
        // 0 0 1    => 1
        // 1 1 0    => 1
        // 0 1 1    => 1
        // 1 0 1    => 1
        // 1 1 1    => 1
    }
}
