using UnityEngine;

public class Pendulum : MonoBehaviour
{
    public Transform pivot;
    public float maxAngle = 10f;
    public float speed = 100f;

    float currentAngle = 0f;
    float localTime = 0f;

    void Update()
    {
        float currentSpeed = speed;

        if (ManagersRoot.instance.abilityManager.abilityIsActive &&
            ManagersRoot.instance.abilityManager.ability == AbilityType.TIME_SLOW)
        {
            currentSpeed *= ManagersRoot.instance.abilityManager.slowTimeFactor;
        }

        localTime += Time.deltaTime * currentSpeed;

        float targetAngle = Mathf.Sin(localTime) * maxAngle;
        float deltaAngle = targetAngle - currentAngle;

        currentAngle = targetAngle;

        transform.RotateAround(pivot.position, Vector3.forward, deltaAngle);
    }
}
