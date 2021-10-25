using InputManagement;
using UnityEngine;

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

    public float mutationAmount = .15f;

    [SerializeField]
    private string networkName;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private PlayerInputController playerInputController;
    private NetworkInputController networkInputController;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        playerInputController = new PlayerInputController();
        networkInputController = new NetworkInputController(this, networkName);
    }

    private void FixedUpdate() {
        InputController ic = usePlayerInput ? (InputController) playerInputController : (InputController) networkInputController;
        Vector2 input = ic.GetInput();
        sr.sprite = input.x < 0f ? left : input.x > 0f ? right : forward;
        float upDotVelocity = Vector2.Dot(transform.up, rb.velocity);
        Move(input, upDotVelocity);
        Turn(input, upDotVelocity);
        ControlDrift(upDotVelocity);
    }

    private void Move(in Vector2 input, in float upDotVelocity) {
        if((input.y > 0 && upDotVelocity > maximumSpeed) || (input.y < 0 && upDotVelocity < -maximumSpeed))
            return;
        rb.AddForce(transform.up * acceleration * input.y);
    }

    private void Turn(in Vector2 input, in float upDotVelocity) {
        float turnFactor = Mathf.Clamp01(Mathf.Abs(upDotVelocity) / fullSteerAtVelocity);
        float invert = Mathf.Sign(upDotVelocity);
        rb.MoveRotation(rb.rotation - rotationStrength * input.x * turnFactor * invert);
    }

    private void ControlDrift(in float upDotVelocity) {
        Vector2 forwardVelocity = transform.up * upDotVelocity;
        Vector2 sidewaysVelocity = transform.right * Vector2.Dot(transform.right, rb.velocity);
        rb.velocity = forwardVelocity + driftStrength * sidewaysVelocity;
    }

    public void Save() {
        networkInputController?.Save(networkName);
    }

}
