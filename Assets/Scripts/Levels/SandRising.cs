using UnityEngine;

public class SandRising : MonoBehaviour
{
    public float riseSpeed = 0.5f;   
    public float maxHeight = 10f;    
    private Vector3 startScale;
    private Material sandMat;

    void Start()
    {
        sandMat = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        float newHeight = transform.localScale.y + riseSpeed * Time.deltaTime;
        sandMat.mainTextureOffset += new Vector2(0, Time.deltaTime * 0.1f);

        if (newHeight <= maxHeight)
        {
            transform.localScale = new Vector3(
                transform.localScale.x,
                newHeight,
                transform.localScale.z
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
