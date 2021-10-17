using NN;
using LinearAlgebra;
using System;
using UnityEngine;

public class Main : MonoBehaviour {

    private void Start() {
        TestNetwork();
        TestAlgebra();
        TestErrors();
    }

    private void TestNetwork() {
        NeuralNetwork nn = new NeuralNetwork(new int[] {4, 2, 3});
        nn.Save("test");
        nn = NeuralNetwork.Load("test");
        Debug.Log(nn);
    }

    private void TestAlgebra() {
        Vector vector1 = new Vector(new float[] {1, 2, 3, 4});
        Vector vector2 = new Vector(new float[] {-1, 0, .5f, 1});
        Vector vector = new Vector(new float[] {3, 4});
        Matrix matrix = new Matrix(new float[][] {new float[] {1, 2, 3}, new float[] {-1, 0, 1}});
        Debug.Log(2 * vector1);
        Debug.Log(vector1 + vector2);
        Debug.Log(matrix * vector);
        Debug.Log(matrix);
    }

    private void TestErrors() {
        try {
            new Vector(new float[0]);
        } catch(ArgumentException e) {
            Debug.Log(e);
        }

        try {
            new Matrix(new float[0][]);
        } catch(ArgumentException e) {
            Debug.Log(e);
        }

        try {
            new Matrix(new float[][] {new float[0]});
        } catch(ArgumentException e) {
            Debug.LogError(e);
        }
    }

}
