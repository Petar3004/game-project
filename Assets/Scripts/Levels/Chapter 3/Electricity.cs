using System.Collections;
using UnityEngine;

public class Electricity : MonoBehaviour
{
    public bool red;
    public bool blue;
    public bool green;

    public float greenOnTime = 3f;
    public float greenOffTime = 4f;

    private SpriteRenderer sprite;
    private Collider2D col;

    private int damage = 1;
    private bool disabledByPlayer = false;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        if (green)
        {
            StartCoroutine(GreenRoutine());
        }
        else
        {
            EnableElectricity();
        }
    }

    private IEnumerator GreenRoutine()
    {
        while (true)
        {
            EnableElectricity();
            yield return new WaitForSeconds(greenOnTime);

            DisableElectricity();
            yield return new WaitForSeconds(greenOffTime);
        }
    }

    private void OnMouseDown()
    {
        if (!blue || disabledByPlayer) return;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            DisableElectricity();
            disabledByPlayer = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!sprite.enabled) return;

        if (other.CompareTag("PlayerObject") && (red || green))
        {
            ManagersRoot.instance.gameManager.RestartLevel();
            ManagersRoot.instance.audioManager.PlaySFX(ManagersRoot.instance.audioManager.death);
            other.GetComponentInChildren<PlayerHealth>().TakeDamage(damage);
        }
    }

    private void EnableElectricity()
    {
        sprite.enabled = true;
        col.enabled = true;
    }

    private void DisableElectricity()
    {
        sprite.enabled = false;
        col.enabled = false;
    }
}
