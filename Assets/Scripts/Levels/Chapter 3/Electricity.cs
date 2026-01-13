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

    private void Update()
    {
        // Only BLUE electricity is affected by the ability
        if (type != ElectricityType.BLUE)
            return;

        bool electricityDisableActive =
            ManagersRoot.instance.abilityManager.abilityIsActive &&
            ManagersRoot.instance.abilityManager.ability == AbilityType.ELECTRICITY_DISABLE;

        if (electricityDisableActive)
            DisableElectricity();
        else
            EnableElectricity();
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
