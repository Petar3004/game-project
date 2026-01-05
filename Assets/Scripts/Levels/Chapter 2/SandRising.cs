using UnityEngine;

public class SandRising : MonoBehaviour
{
    public float riseSpeed = 0.5f;
    public float widthFillSpeed = 0.2f;
    public float maxHeight = 10f;
    public int targetRoomIndex;

    private BoxCollider2D sandCollider;
    private SpriteRenderer sandRenderer;
    private Material sandMat;
    private Vector2 startSize;
    private float startY;
    private bool isRising = false;

    public float maxWidth = 20f;
    public bool growWidth = false;

    private float startX;
    private float startWidth;



    void OnEnable()
    {
        CameraController.OnRoomChanged += HandleRoomChanged;
    }

    void OnDisable()
    {
        CameraController.OnRoomChanged -= HandleRoomChanged;
    }

    void Start()
    {
        sandCollider = GetComponent<BoxCollider2D>();
        sandRenderer = GetComponent<SpriteRenderer>();
        sandMat = sandRenderer.material;

        startSize = sandCollider.size;
        startWidth = startSize.x;

        startY = transform.localPosition.y;
        startX = transform.localPosition.x;
    }

    void Update()
    {
        if (!isRising) return;

        float newHeight = sandCollider.size.y;
        float newWidth = sandCollider.size.x;

        // Height growth
        if (newHeight < maxHeight)
        {
            newHeight += riseSpeed * Time.deltaTime;
        }

        // Width growth (left → right)
        if (growWidth && newWidth < maxWidth)
        {
            newWidth += widthFillSpeed * Time.deltaTime;
        }

        // Apply size
        sandRenderer.size = new Vector2(newWidth, newHeight);
        sandCollider.size = new Vector2(newWidth, newHeight);

        // Anchor bottom
        float yPos = startY + newHeight / 2f;

        // Anchor left edge
        float xPos = startX + (newWidth - startWidth) / 2f;

        transform.localPosition = new Vector3(xPos, yPos, transform.localPosition.z);

        // Texture scroll
        sandMat.mainTextureOffset += new Vector2(0, Time.deltaTime * 0.1f);
    }



    private void HandleRoomChanged(int newRoomIndex)
    {
        if (newRoomIndex == targetRoomIndex)
        {
            StartRising();
            growWidth = (newRoomIndex == 3);
        }
        else
        {
            StopRising();
            growWidth = false;
        }
    }


    public void StartRising()
    {
        isRising = true;
    }

    public void StopRising()
    {
        isRising = false;
    }
}
