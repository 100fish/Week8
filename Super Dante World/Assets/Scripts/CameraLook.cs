using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] float sensitivity;
    #region inputSetup
    private InputMap inputMap;

    private void Awake()
    {
        inputMap = new InputMap();
    }

    private void OnEnable()
    {

        inputMap.Enable();
    }

    private void OnDisable()
    {
        inputMap.Disable();
    }
    #endregion
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {

        transform.RotateAround(target.position, Vector3.up, inputMap.Standard.Look.ReadValue<Vector2>().x * sensitivity);
        transform.RotateAround(target.position, transform.right, inputMap.Standard.Look.ReadValue<Vector2>().y * sensitivity);

        transform.LookAt(target.position, Vector2.up);
    }
}
