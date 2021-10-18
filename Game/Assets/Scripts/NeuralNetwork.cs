using LinearAlgebra;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace NN {

    public static class Utility {

        #region IO
        private static BinaryFormatter _formatter;  //don't refer to this
        private static BinaryFormatter formatter {
            get => _formatter is null ? _formatter = new BinaryFormatter() : _formatter;    //creates instance on first reference
        }

        private static string GetPath(in string fileName) {
            return $"{Application.dataPath}/{fileName}";
        }

        public static void Save<T>(in string fileName, in T data) {
            if(data is null)
                return;
            FileStream stream = File.Open(GetPath(fileName), FileMode.Create);
            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static object Load(in string fileName) {
            string path = GetPath(fileName);
            if(!File.Exists(path))
                return default;
            FileStream stream = new FileStream(path, FileMode.Open);
            object data = formatter.Deserialize(stream);
            stream.Close();
            return data;
        }
        #endregion

    }

    [System.Serializable]
    public class NeuralNetwork {

        private Vector[] biases;

        private Matrix[] weights;

        public NeuralNetwork(in NeuralNetwork network) {
            biases = new Vector[network.biases.Length];
            weights = new Matrix[network.weights.Length];
            for(int i = 0; i < network.biases.Length; i++) {
                biases[i] = new Vector(network.biases[i]);
                weights[i] = new Matrix(network.weights[i]);
            }
        }

        public NeuralNetwork(in int[] layers) {
            if(layers.Length < 2)
                throw new System.ArgumentException("Network Size is invalid", nameof(layers));
            int arrsLength = layers.Length - 1;
            biases = new Vector[arrsLength];
            weights = new Matrix[arrsLength];
            for(int i = 0; i < arrsLength; i++) {
                int size = layers[i + 1];
                biases[i] = new Vector(InitArray(size));
                weights[i] = new Matrix(InitTable(size, layers[i]));
            }

            float[] InitArray(in int size) {
                float[] array = new float[size];
                for(int i = 0; i < size; i++)
                    array[i] = Random.Range(-.3f, .3f);
                return array;
            }

            float[][] InitTable(in int size, in int previousSize) {
                float[][] table = new float[previousSize][];
                for(int i = 0; i < previousSize; i++)
                    table[i] = InitArray(size);
                return table;
            }
        }

        public Vector FeedForward(Vector activations) {
            for(int i = 0; i < biases.Length; i++)
                activations = weights[i] * activations + biases[i];
            return activations;
        }

        public void Mutate(in float amount) {
            float max = .5f * amount, min = -max;
            for(int i = 0; i < biases.Length; i++) {
                for(int j = 0; j < biases[i].Size; j++)
                    biases[i][j] += Random.Range(min, max);
                for(int j = 0; j < weights[i].Size[1]; j++)
                    for(int k = 0; k < weights[i].Size[0]; k++)
                        weights[i][j, k] += Random.Range(min, max);
            }
        }

        #region IO-Methods
        public static NeuralNetwork Load(in string name) {
            return Utility.Load(GetFileName(name)) as NeuralNetwork;
        }

        public void Save(in string name) {
            Utility.Save(GetFileName(name), this);
        }

        private static string GetFileName(in string name) {
            return $"Saves/{name}.net";
        }
        #endregion

        public override string ToString() {
            string layers = $"{weights[0].Size[1]}";
            foreach(Vector layer in biases)
                layers += $", {layer.Size}";
            return $"Network: {layers}";
        }

    }

}
