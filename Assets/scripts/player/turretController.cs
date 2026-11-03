using UnityEngine;
using UnityEngine.InputSystem;

public class TurretController : MonoBehaviour
{
    [SerializeField] private Transform leftTurret;
    [SerializeField] private Transform rightTurret;

    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float maxAngle = 60f;
    [SerializeField] private GameObject projectilePrefab;

    [SerializeField] private Transform leftFirePoint;
    [SerializeField] private Transform rightFirePoint;
    [SerializeField] private float fireRate = 0.2f;

    private bool leftFiring;
    private bool rightFiring;

    private float leftNextShot;
    private float rightNextShot;

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

        if (leftFiring && Time.time >= leftNextShot)
        {
            Fire(leftFirePoint, -1);
            leftNextShot = Time.time + fireRate;
        }

        if (rightFiring && Time.time >= rightNextShot)
        {
            Fire(rightFirePoint, 1);
            rightNextShot = Time.time + fireRate;
        }
    }

    public void LeftAim(InputAction.CallbackContext context)
    {
        leftInput = context.ReadValue<float>();
    }

    public void RightAim(InputAction.CallbackContext context)
    {
        rightInput = context.ReadValue<float>();
    }

    public void LeftFire(InputAction.CallbackContext context)
    {
        leftFiring = context.ReadValueAsButton();
    }

    public void RightFire(InputAction.CallbackContext context)
    {
        rightFiring = context.ReadValueAsButton();
    }

    private void Fire(Transform firePoint, int side)
    {
        if (projectilePrefab == null || firePoint == null)
            return;

        GameObject bullet = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        Vector2 direction = firePoint.right * side;

        bullet.GetComponent<Projectile>().SetDirection(direction);
    }
}