using InputManagement;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private Sprite forward, left, right;

    [Header("Movement")]

    [SerializeField]
    private float acceleration = 20f;

    [SerializeField]
    private float maximumSpeed = 15f;
    
    [SerializeField]
    private float rotationStrength = 3f;

    [SerializeField]
    private float fullSteerAtVelocity = 10f;

    [SerializeField] [Range(0, 1)]
    private float driftStrength = .9f;

    [Header("Input")]

    [SerializeField]
    private bool usePlayerInput = true;

    [SerializeField]
    private float timeToRace = 1f;
    private float raced;
    private int bestIndex;
    private float bestScore = float.MinValue;

    public float mutationAmount = .15f;

    [SerializeField]
    private string networkName;

    private Vector2 startPosition;
    private Quaternion startRotation;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private GoalManager gm;
    private PlayerInputController playerInputController;
    private NetworkInputController networkInputController;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        gm = GameObject.Find("Goals").GetComponent<GoalManager>();
        playerInputController = new PlayerInputController();
        networkInputController = new NetworkInputController(this, networkName);
    }

    private void Start() {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    private void FixedUpdate() {
        if(!usePlayerInput) {
            raced += Time.fixedDeltaTime;
            if(raced > timeToRace)
                Reset();
        }

        InputController ic = usePlayerInput ? (InputController) playerInputController : (InputController) networkInputController;
        Vector2 input = ic.GetInput();
        sr.sprite = input.x < 0f ? left : input.x > 0f ? right : forward;
        float upDotVelocity = Vector2.Dot(transform.up, rb.velocity);
        Move(input, upDotVelocity);
        Turn(input, upDotVelocity);
        ControlDrift(upDotVelocity);

        void Reset() {
            float score = gm.GetScore(transform);
            if(score > bestScore) {
                bestScore = score;
                bestIndex = networkInputController.index;
            }
            networkInputController.index++;
            if(networkInputController.index == NetworkInputController.generationSize) {
                Debug.LogWarning($"Top Score: {bestScore}");
                bestScore = float.MinValue;
                networkInputController.index = 0;
                networkInputController.GenerateGeneration(networkInputController[bestIndex]);
            }
            gm.Reset();
            raced = 0f;
            transform.position = startPosition;
            transform.rotation = startRotation;
            rb.velocity = Vector2.zero;
        }

        void Move(in Vector2 input, in float upDotVelocity) {
            if((input.y > 0 && upDotVelocity > maximumSpeed) || (input.y < 0 && upDotVelocity < -maximumSpeed))
                return;
            rb.AddForce(transform.up * acceleration * input.y);
        }

        void Turn(in Vector2 input, in float upDotVelocity) {
            float turnFactor = Mathf.Clamp01(Mathf.Abs(upDotVelocity) / fullSteerAtVelocity);
            float invert = Mathf.Sign(upDotVelocity);
            rb.MoveRotation(rb.rotation - rotationStrength * input.x * turnFactor * invert);
        }

        void ControlDrift(in float upDotVelocity) {
            Vector2 forwardVelocity = transform.up * upDotVelocity;
            Vector2 sidewaysVelocity = transform.right * Vector2.Dot(transform.right, rb.velocity);
            rb.velocity = forwardVelocity + driftStrength * sidewaysVelocity;
        }
    }

    public void Save() {
        networkInputController?.Save(0, networkName);
    }

}
