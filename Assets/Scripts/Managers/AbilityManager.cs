using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum AbilityType
{
    TIME_SLOW,
    SAND_SPEED,
}

public class AbilityManager : MonoBehaviour
{
    public AbilityType ability;
    private Dictionary<AbilityType, List<int>> abilityToLevels = new Dictionary<AbilityType, List<int>>
    {
        { AbilityType.TIME_SLOW, new List<int> { 1, 2, 3 } },
        { AbilityType.SAND_SPEED, new List<int> { 4, 5, 6 } }
    };

    public float abilityDuration = 5f;
    public float abilityTimePenalty = 10f;
    public bool abilityIsActive = false;
    public float abilityCharge = 1f;

    [Header("Slow Time")]
    public float slowTimeFactor = 0.5f;

    void Update()
    {
        bool gamePaused = ManagersRoot.instance.pauseManager.isPaused;
        bool gameStarted = ManagersRoot.instance.gameManager.gameStarted;

        if (Input.GetKeyDown(KeyCode.LeftShift) && gameStarted && !gamePaused)
        {
            ActivateAbility();
        }

        UIRoot.instance.UpdateAbiliyUI();
    }

    public void UpdateAbility()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        foreach (AbilityType abilityType in abilityToLevels.Keys)
        {
            if (abilityToLevels[abilityType].Contains(currentLevelIndex))
            {
                ability = abilityType;
            }
        }
    }

    public void ActivateAbility()
    {
        if (!abilityIsActive && ManagersRoot.instance.timeManager.timeLeft > abilityTimePenalty)
        {
            ManagersRoot.instance.timeManager.timeLeft -= abilityTimePenalty;
            StartCoroutine(StartAbility());
        }
    }

    private IEnumerator StartAbility()
    {
        abilityIsActive = true;
        float elapsed = 0f;
        while (elapsed < abilityDuration && abilityIsActive)
        {
            elapsed += Time.deltaTime;
            abilityCharge = elapsed / abilityDuration;
            yield return null;
        }

        abilityCharge = 1f;
        abilityIsActive = false;
    }
}
