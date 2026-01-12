using System.Collections;
using UnityEngine;

public enum ElectricityType
{
    RED,
    BLUE,
    GREEN
}

public class Electricity : MonoBehaviour
{
    public ElectricityType type;

    [Header("Green Settings")]
    public float greenOnTime = 3f;
    public float greenOffTime = 4f;

    private SpriteRenderer sprite;
    private Collider2D col;

    private int damage = 1;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        switch (type)
        {
            case ElectricityType.GREEN:
                StartCoroutine(GreenRoutine());
                break;

            case ElectricityType.RED:
            case ElectricityType.BLUE:
                EnableElectricity();
                break;
        }
    }

    /*
    private void Update()
    {
        if (type == ElectricityType.BLUE)
        {
            bool disableActive =
                ManagersRoot.instance.abilityManager.abilityIsActive &&
                ManagersRoot.instance.abilityManager.ability == AbilityType.ELECTRICITY_DISABLE;

            if (disableActive)
                DisableElectricity();
            else
                EnableElectricity();
        }
    }
    */

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!sprite.enabled) return;

        if (other.CompareTag("Player"))
        {
            ManagersRoot.instance.audioManager.PlaySFX(
                ManagersRoot.instance.audioManager.death
            );

            other.GetComponentInChildren<PlayerHealth>().TakeDamage(damage);
            ManagersRoot.instance.gameManager.RestartLevel();
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
