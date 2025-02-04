using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GliderController : MonoBehaviour
{
    public float lift = 1.0f; // Public lift variable
    public float pitchSpeed = 10.0f; // Speed of pitch adjustment
    public float rollSpeed = 10.0f; // Speed of roll adjustment
    public float yawSpeed = 10.0f; // Speed of yaw adjustment
    public float damping = 2.0f; // Damping factor for easing back to level
    public Vector3 startVelocity = new Vector3(0, 0, 10); // Public start velocity variable
    public float stallVelocity = 5f;
    
    public Rigidbody rb;
    private float _pitchInput;
    private float _rollInput;
    private float _yawInput;

    void Start()
    {
        rb.linearVelocity = startVelocity; // Set the initial velocity
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 20), rb.linearVelocity.magnitude.ToString());
    }

    void FixedUpdate()
    {
        // Calculate the lift force
        float velocityMagnitude = rb.linearVelocity.magnitude;
        float stallFactor = Mathf.Min(stallVelocity, velocityMagnitude) / stallVelocity; // stall if velocity falls below stallVelocity
        Vector3 liftForce = transform.up * (lift * rb.mass * Physics.gravity.magnitude * stallFactor);

        // Get the pitch and roll angles in radians
        float pitch = Mathf.Deg2Rad * transform.eulerAngles.x;
        float roll = Mathf.Deg2Rad * transform.eulerAngles.z;

        // Adjust the lift force based on pitch and roll
        if (transform.eulerAngles.x < 90f) pitch = 0f;
        float liftAdjustment = Mathf.Cos(pitch) * Mathf.Cos(roll);
        liftForce *= (liftAdjustment * liftAdjustment); // square it to soften the effect

        // Apply the lift force to counteract gravity
        rb.AddForce(liftForce, ForceMode.Force);

        // Adjust pitch, roll, and yaw based on user input
        AdjustPitch(_pitchInput);
        AdjustRoll(_rollInput);
        AdjustYaw(_yawInput);

        // Apply damping to ease back to level
        ApplyDamping();
    }
    
    public void GetPitchInput(InputAction.CallbackContext context)
    {
        _pitchInput = context.ReadValue<float>();
    }

    public void GetRollInput(InputAction.CallbackContext context)
    {
        _rollInput = -context.ReadValue<float>();
    }

    public void GetYawInput(InputAction.CallbackContext context)
    {
        _yawInput = context.ReadValue<float>();
    }

    private void AdjustPitch(float amount)
    {
        transform.Rotate(Vector3.right, amount * pitchSpeed * Time.deltaTime);
    }

    private void AdjustRoll(float amount)
    {
        transform.Rotate(Vector3.forward, amount * rollSpeed * Time.deltaTime);
    }

    private void AdjustYaw(float amount)
    {
        transform.Rotate(Vector3.up, amount * yawSpeed * Time.deltaTime);
    }
    
    private void ApplyDamping()
    {
        // Apply damping to pitch
        if (Mathf.Approximately(_pitchInput, 0))
        {
            float pitchAngle = NormalizeAngle(transform.eulerAngles.x);
            float pitchCorrection = -pitchAngle * damping * Time.deltaTime;
            transform.Rotate(Vector3.right, pitchCorrection);
        }

        // Apply damping to roll
        if (Mathf.Approximately(_rollInput, 0))
        {
            float rollAngle = NormalizeAngle(transform.eulerAngles.z);
            float rollCorrection = -rollAngle * damping * Time.deltaTime;
            transform.Rotate(Vector3.forward, rollCorrection);
        }

        // Apply damping to yaw
        if (Mathf.Approximately(_yawInput, 0))
        {
            float yawAngle = NormalizeAngle(transform.eulerAngles.y);
            float yawCorrection = -yawAngle * damping * Time.deltaTime;
            transform.Rotate(Vector3.up, yawCorrection);
        }
    }

    private float NormalizeAngle(float angle)
    {
        if (angle > 180f)
        {
            angle -= 360f;
        } 
        return angle;
    }
}