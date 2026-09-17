using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [SerializeField] WheelCollider FLWheelCollider;
    [SerializeField] WheelCollider BLWheelCollider;
    [SerializeField] WheelCollider BRWheelCollider;
    [SerializeField] WheelCollider FRWheelCollider;

    [SerializeField] Transform FLWheel;
    [SerializeField] Transform BLWheel;
    [SerializeField] Transform BRWheel;
    [SerializeField] Transform FRWheel;

    AudioSource carSound;
    Rigidbody rb;

    [SerializeField] float motorForce;
    [SerializeField] float breakForce;
    [SerializeField] float currentBreakForce;

    bool isBreaking = false;
    bool hasPlayed = false;

    [SerializeField] float steerAngle;
    [SerializeField] float maxSteerAngle;

    #region inputSetup
    private InputMap inputMap;

    private void Awake()
    {
        inputMap = new InputMap();
    }

    private void OnEnable()
    {
        
        inputMap.Enable();
        inputMap.Standard.Fix.performed += FixCar;
    }

    private void OnDisable()
    {
        inputMap.Disable();
        inputMap.Standard.Fix.performed -= FixCar;
    }
    #endregion

    private void Start()
    {
        carSound = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixCar(InputAction.CallbackContext context)
    {
        transform.rotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void HandleMotor()
    {
        FLWheelCollider.motorTorque = inputMap.Standard.Move.ReadValue<Vector2>().y * motorForce;
        FRWheelCollider.motorTorque = inputMap.Standard.Move.ReadValue<Vector2>().y * motorForce;

        carSound.pitch = (rb.linearVelocity.x + rb.linearVelocity.y) / 2 / 5;

        currentBreakForce = isBreaking ? breakForce : 0;
        if (isBreaking)
        {
            FLWheelCollider.brakeTorque = currentBreakForce;
            BLWheelCollider.brakeTorque = currentBreakForce;
            BRWheelCollider.brakeTorque = currentBreakForce;
            FRWheelCollider.brakeTorque = currentBreakForce;
        }
    }
    private void HandleSteering()
    {
        steerAngle = maxSteerAngle * inputMap.Standard.Move.ReadValue<Vector2>().x;
        FLWheelCollider.steerAngle = steerAngle;
        FRWheelCollider.steerAngle = steerAngle;
    }

    private void UpdateWheels()
    {
        UpdateWheel(FRWheel, FRWheelCollider);
        UpdateWheel(BRWheel, BRWheelCollider);
        UpdateWheel(BLWheel, BLWheelCollider);
        UpdateWheel(FLWheel, FLWheelCollider);
    }

    private void UpdateWheel(Transform wheel, WheelCollider wheelC)
    {
        Vector3 pos;
        Quaternion rot;
        wheelC.GetWorldPose(out pos, out rot);
        wheel.position = pos;
        wheel.rotation = rot;
    }
}
