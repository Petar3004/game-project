using UnityEngine;

public abstract class GearMovement : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 3f;
    public bool movingPositive;
    protected Vector3 startPosition;
    protected float minBound;
    protected float maxBound;

    protected virtual void Start()
    {
        startPosition = transform.position;
        InitializeBounds();
    }
    void Update()
    {
        Move();
    }

    protected abstract void InitializeBounds();
    protected abstract void Move();

}