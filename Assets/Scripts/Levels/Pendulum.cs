using UnityEngine;

public class Pendulum : MonoBehaviour
{
    public float amplitude = 6f;
    public float height = 0.3f;
    public float speed = 0.7f;

    private Vector3 startPos;
    private float localTime = 0f;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float currentSpeed = speed;

        if (ManagersRoot.instance.timeManager.isSlowed)
        {
            currentSpeed *= ManagersRoot.instance.timeManager.slowTimeFactor;
        }

        localTime += Time.deltaTime * currentSpeed;

        float x = Mathf.Sin(localTime) * amplitude;
        float y = Mathf.Pow(x / amplitude, 2) * height + startPos.y;

        transform.position = startPos + new Vector3(x, y, 0);
    }
}
