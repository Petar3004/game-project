using UnityEngine;

public class SandRising : MonoBehaviour
{
    public float riseSpeed = 0.5f;   // units per second
    public float maxHeight = 10f;    // room height
    private Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        float newHeight = transform.localScale.y + riseSpeed * Time.deltaTime;

        if (newHeight <= maxHeight)
        {
            transform.localScale = new Vector3(
                startScale.x,
                newHeight,
                startScale.z
            );
        }

        GetComponent<SpriteRenderer>().material.mainTextureOffset += new Vector2(0, Time.deltaTime * 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerObject"))
        {
            ManagersRoot.instance.gameManager.RestartLevel();
        }
    }
}
