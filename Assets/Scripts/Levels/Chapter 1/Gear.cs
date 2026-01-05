using System.Collections;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public bool isStatic = true;
    public float spinSpeed = 1f;
    [Header("Move")]
    public float moveSpeed = 2f;
    public float distance = 3f;
    [Range(0f, 360f)]
    public float angle = 0;
    public float pauseTimeSeconds = 0;
    private Vector3 startPosition;
    private bool currentDirection;
    private bool lastDirection;
    private Vector3 pointA;
    private Vector3 pointB;


    void Start()
    {
        startPosition = transform.position;
        if (!isStatic)
        {
            InitializeBounds();
            StartCoroutine(Move());
        }
    }

    void Update()
    {
        float currentSpinSpeed = spinSpeed;
        if (ManagersRoot.instance.abilityManager.abilityIsActive && ManagersRoot.instance.abilityManager.ability == AbilityType.TIME_SLOW)
        {
            currentSpinSpeed *= ManagersRoot.instance.abilityManager.slowTimeFactor;
        }
        transform.Rotate(0, 0, currentSpinSpeed * Time.deltaTime);
    }

    void InitializeBounds()
    {
        float angleRad = angle * Mathf.Deg2Rad;

        pointA = startPosition;
        pointB = startPosition + new Vector3(
            Mathf.Cos(angleRad) * distance,
            Mathf.Sin(angleRad) * distance
        );
    }


    private IEnumerator Move()
    {
        while (true)
        {
            if (lastDirection != currentDirection)
            {
                yield return new WaitForSecondsRealtime(pauseTimeSeconds);
                lastDirection = currentDirection;
            }
            MoveInOneDirection();
            yield return null;
        }
    }

    private void MoveInOneDirection()
    {
        Vector3 pos = transform.position;
        Vector3 target = currentDirection ? pointB : pointA;

        float currentSpeed = moveSpeed;
        if (ManagersRoot.instance.abilityManager.abilityIsActive && ManagersRoot.instance.abilityManager.ability == AbilityType.TIME_SLOW)
            currentSpeed *= ManagersRoot.instance.abilityManager.slowTimeFactor;

        transform.position = Vector3.MoveTowards(pos, target, currentSpeed * Time.deltaTime);

        if (transform.position == target)
        {
            currentDirection = !currentDirection;
        }
    }
}