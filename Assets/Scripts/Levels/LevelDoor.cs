using System.Collections;
using UnityEngine;

public class LevelDoor : MonoBehaviour
{
    public SpriteRenderer front;
    public SpriteRenderer back;
    public SpriteRenderer backBack;
    public float spinSpeed = 1;
    public float attractSpeed = 0.3f;
    private Coroutine animationRoutine = null;
    public bool chapterEnd = false;
    public bool gameOver = false;

    void Update()
    {
        front.transform.Rotate(0, 0, spinSpeed * 2 * Time.deltaTime);
        back.transform.Rotate(0, 0, -spinSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || animationRoutine != null)
        {
            return;
        }

        animationRoutine = StartCoroutine(PortalAnimation());
    }

    private IEnumerator PortalAnimation()
    {
        GameObject player = ManagersRoot.instance.playerManager.Player;
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        player.GetComponent<PlayerMovement>().LockPosition(true);
        rb.gravityScale = 0;
        rb.linearVelocity = Vector3.zero;

        while (Vector2.Distance(rb.position, transform.position) > 0.05f)
        {
            rb.position = Vector2.MoveTowards(
                rb.position,
                transform.position,
                attractSpeed * Time.deltaTime
            );

            player.transform.localScale = Vector3.MoveTowards(
                player.transform.localScale,
                Vector3.zero,
                attractSpeed * Time.deltaTime
            );

            yield return null;
        }

        ManagersRoot.instance.audioManager.PlaySFX(
            ManagersRoot.instance.audioManager.levelComplete
        );
        if (!chapterEnd && !gameOver)
        {
            ManagersRoot.instance.sceneController.GoToNextLevel();
        }
        else if (chapterEnd)
        {
            ManagersRoot.instance.sceneController.GoToMainMenu();
            ManagersRoot.instance.gameManager.chapterComplete = true;
        }
        else if (gameOver)
        {
            ManagersRoot.instance.sceneController.GoToCutscene(16);
        }
    }

    void OnValidate()
    {
        if (chapterEnd || gameOver)
        {
            front.color = Color.gold;
            back.color = Color.gold;
            backBack.color = Color.gold;
            gameObject.SetActive(false);
        }
        else
        {
            front.color = Color.white;
            back.color = Color.white;
            backBack.color = Color.white;
            gameObject.SetActive(true);
        }
    }
}
