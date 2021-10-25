using LinearAlgebra;
using NN;
using UnityEngine;

namespace InputManagement {

    public interface InputController {

        Vector2 GetInput();

    }

    public class PlayerInputController : InputController {

        Vector2 InputController.GetInput() {
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

    }

    public class NetworkInputController : InputController {

        private int[] layers = new int[] {7, 12, 8, 4, 2};

        public int index = 0;
        public const int generationSize = 50;
        private NeuralNetwork[] generation;

        private GoalManager gm;
        private PlayerController controller;
        private Rigidbody2D rb;

        public NetworkInputController(in PlayerController controller, in string name) {
            gm = GameObject.Find("Goals").GetComponent<GoalManager>();
            this.controller = controller;
            rb = controller.GetComponent<Rigidbody2D>();
            NeuralNetwork neuralNetwork = NeuralNetwork.Load(name);
            if(neuralNetwork is null || neuralNetwork.InputSize != layers[0])
                neuralNetwork = new NeuralNetwork(layers);
            GenerateGeneration(neuralNetwork);
        }

        private void GenerateGeneration(in NeuralNetwork neuralNetwork) {
            generation = new NeuralNetwork[generationSize];
            for(int i = 0; i < generationSize; i++)
                (generation[i] = new NeuralNetwork(neuralNetwork)).Mutate(controller.mutationAmount);
        }

        Vector2 InputController.GetInput() {
            float[] inputs = GetInputLayer();
            return Convert(generation[index].FeedForward(new Vector(inputs)));

            float[] GetInputLayer() {
                float[] inputs = new float[layers[0]];
                inputs[0] = Vector2.Dot(controller.transform.up, rb.velocity);
                inputs[1] = Vector2.Dot(controller.transform.right, rb.velocity);
                inputs[2] = rb.rotation;
                Vector2 position = rb.position;
                inputs[3] = position.x;
                inputs[4] = position.y;
                Vector2 nextGoal = gm.NextGoal;
                inputs[5] = nextGoal.x;
                inputs[6] = nextGoal.y;
                return inputs;
            }

            Vector2 Convert(in Vector vector) {
                return new Vector2(vector[0], vector[1]);
            }
        }

        public void Save(in string name) {
            generation[index].Save(name);
        }

    }

}
