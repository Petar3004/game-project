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
    public static AbilityManager instance;
    public AbilityType ability;
    public Image abilityUI;
    private Color col;
    private Dictionary<AbilityType, List<int>> abilityToLevels = new Dictionary<AbilityType, List<int>>
    {
        { AbilityType.TIME_SLOW, new List<int> { 1, 2, 3 } },
        { AbilityType.SAND_SPEED, new List<int> { 4, 5, 6 } }
    };

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        col = abilityUI.color;
    }

    void Update()
    {
        if (TimeManager.instance.isSlowed)
        {
            col.a = 0.5f;
            abilityUI.color = col;
        }
        else
        {
            col.a = 1f;
            abilityUI.color = col;
        }
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
}
