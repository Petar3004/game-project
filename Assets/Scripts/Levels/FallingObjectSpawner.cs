using UnityEngine;

public class FallingObjectSpawner : MonoBehaviour
{
    public FallingObject fallingObject;
    public float avgSpawnTime = 3f;
    public float timeRange = 0.5f;
    public float avgGravity = 0.5f;
    public float gravityRange = 0.2f;
    public float posRange = 5f;
    public float scaleRange = 0.5f;
    public int nthCollectable = 5;
    private int n = 0;
    private float spawnTime;
    private Vector3 spawnPos;
    private float scaleFactor;
    private float rotation;
    private float gravity;
    private FallingObjectType objType;

    private float elapsed;

    private void Update()
    {
        if (elapsed >= spawnTime)
        {
            UpdateRandoms();
            SpawnObject();
            elapsed = 0;
        }
        elapsed += Time.deltaTime;
    }

    private void SpawnObject()
    {

        n++;
        FallingObject newFallingObject = Instantiate(fallingObject, spawnPos, Quaternion.Euler(new Vector3(0, 0, rotation)));
        Rigidbody2D objRb = newFallingObject.GetComponent<Rigidbody2D>();
        newFallingObject.transform.localScale *= scaleFactor;
        newFallingObject.fallingObjectType = objType;
        objRb.gravityScale = gravity;
        if (n % nthCollectable == 0)
        {
            objType = FallingObjectType.COLLECTABLE;
        }
        else
        {
            objType = FallingObjectType.ENEMY;
        }
    }

    private void UpdateRandoms()
    {
        spawnTime = Random.Range(avgSpawnTime * (1 - timeRange), avgSpawnTime * (1 + timeRange));
        spawnPos = transform.position + new Vector3(Random.Range(-posRange, posRange), 0, 0);
        scaleFactor = Random.Range(1f - scaleRange, 1f + scaleRange);
        rotation = Random.Range(0, 360f);
        gravity = Random.Range(avgGravity * (1 - gravityRange), avgGravity * (1 + gravityRange));
    }
}
