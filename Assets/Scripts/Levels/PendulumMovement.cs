using UnityEngine;

public class PendulumMovement : MonoBehaviour
{
    public float amplitude = 6f;
    public float height = 0.3f;
    public float speed = 0.7f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float x = Mathf.Sin(Time.time * speed) * amplitude;
        float y = Mathf.Pow(x / amplitude, 2) * height + startPos.y;
        transform.position = startPos + new Vector3(x, y, 0);
    }
}
