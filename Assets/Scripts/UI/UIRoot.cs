using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum HintType
{
    BIG,
    SMALL
}

public class UIRoot : MonoBehaviour
{
    public static UIRoot instance;

    [Header("Timer")]
    public TMP_Text timerText;
    public Slider timerSlider;

    [Header("Abilities")]
    public TMP_Text abilityText;
    public Image abilityImage;

    [Header("Hints")]
    public GameObject bigHintBox;
    public Image bigHintImage;
    public TMP_Text bigHintText;
    public GameObject smallHintBox;
    public Image smallHintImage;
    public TMP_Text smallHintText;

    [Header("Pause Menu")]
    public GameObject pauseMenu;

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

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void AutoLoad()
    {
        if (instance != null) return;

        UIRoot prefab =
            Resources.Load<UIRoot>("UI");

        if (prefab == null)
        {
            Debug.LogError("UI prefab not found in Resources!");
            return;
        }

        Instantiate(prefab);
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            gameObject.SetActive(false);
        }
        pauseMenu.SetActive(false);
        bigHintBox.SetActive(false);
        smallHintBox.SetActive(false);
    }

    public void SetActiveSpecial()
    {
        gameObject.SetActive(true);
        pauseMenu.SetActive(false);
        bigHintBox.SetActive(false);
        smallHintBox.SetActive(false);
    }

    // Pause Menu
    public void ShowPauseUI()
    {
        pauseMenu.SetActive(true);
    }

    public void HidePauseUI()
    {
        if (pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(false);
        }
    }

    public void OnResumeClicked()
    {
        ManagersRoot.instance.pauseManager.Resume();
    }

    public void OnMainMenuClicked()
    {
        ManagersRoot.instance.pauseManager.MainMenu();
    }

    public void OnQuitClicked()
    {
        ManagersRoot.instance.pauseManager.Quit();
    }

    // Timer
    public void UpdateTimerUI()
    {
        float timeLeft = ManagersRoot.instance.timeManager.timeLeft;
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.CeilToInt(timeLeft).ToString();
        }

        if (timerSlider != null)
        {
            timerSlider.value = timeLeft;
        }
    }

    // Abilities
    public void UpdateAbiliyUI()
    {
        abilityImage.fillAmount = ManagersRoot.instance.abilityManager.abilityCharge;

        switch (ManagersRoot.instance.abilityManager.ability)
        {
            case AbilityType.TIME_SLOW:
                abilityText.text = "Time Magnet";
                break;
            case AbilityType.SAND_SPEED:
                abilityText.text = "Quick Boots";
                break;
        }
    }

    // Hints
    public void ShowHintUI(string hint, HintType type)
    {
        if (type == HintType.SMALL)
        {
            smallHintBox.SetActive(true);
            smallHintText.text = hint;
        }
        else
        {
            bigHintBox.SetActive(true);
            bigHintText.text = hint;
        }
    }

    public void HideHintUI()
    {
        if (smallHintBox.activeInHierarchy)
        {
            smallHintBox.SetActive(false);
        }
        else if (bigHintBox.activeInHierarchy)
        {
            bigHintBox.SetActive(false);
        }
    }
}

