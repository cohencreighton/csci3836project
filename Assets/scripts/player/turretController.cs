using UnityEngine;
using UnityEngine.InputSystem;

public class TurretController : MonoBehaviour
{
    [SerializeField] private Transform leftTurret;
    [SerializeField] private Transform rightTurret;

    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float maxAngle = 60f;

    private float leftInput;
    private float rightInput;

    private float leftAngle;
    private float rightAngle;

    private void Update()
    {
        leftAngle += leftInput * rotationSpeed * Time.deltaTime;
        rightAngle += rightInput * rotationSpeed * Time.deltaTime;

        leftAngle = Mathf.Clamp(leftAngle, -maxAngle, maxAngle);
        rightAngle = Mathf.Clamp(rightAngle, -maxAngle, maxAngle);

        leftTurret.localRotation = Quaternion.Euler(0, 0, leftAngle);
        rightTurret.localRotation = Quaternion.Euler(0, 0, rightAngle);
    }

    public void LeftAim(InputAction.CallbackContext context)
    {
        leftInput = context.ReadValue<float>();
    }

    public void RightAim(InputAction.CallbackContext context)
    {
        rightInput = context.ReadValue<float>();
    }
}