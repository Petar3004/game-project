using UnityEngine;

public class SandFill : MonoBehaviour
{
    public float fillSpeed = 1f; 
    public float maxHeight = 18f; 
    private Vector3 initialScale;
    private float currentFillHeight = 0f;

    void Start()
    {
        initialScale = transform.localScale;
        transform.localScale = new Vector3(initialScale.x, 0f, initialScale.z); 
    }

    void Update()
    {
        if (currentFillHeight < maxHeight)
        {
            currentFillHeight += fillSpeed * Time.deltaTime;
            float fillRatio = Mathf.Clamp01(currentFillHeight / maxHeight);
            transform.localScale = new Vector3(initialScale.x, initialScale.y * fillRatio, initialScale.z);
        }
    }
}
