using InputManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour {

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

    [SerializeField]
    private bool usePlayerInput = true;

    private Rigidbody2D rb;

    private PlayerInputController playerInputController;
    private NetworkInputController networkInputController;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerInputController = new PlayerInputController();
        networkInputController = new NetworkInputController();
    }

    private void FixedUpdate() {
        InputController ic = usePlayerInput ? (InputController) playerInputController : (InputController) networkInputController;
        Vector2 input = ic.GetInput();
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

}
