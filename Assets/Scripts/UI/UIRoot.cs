using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public GameObject hintBox;
    public Image hintImage;
    public TMP_Text hintText;

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
        hintBox.SetActive(false);
    }

    public void SetActiveSpecial()
    {
        gameObject.SetActive(true);
        pauseMenu.SetActive(false);
        hintBox.SetActive(false);
    }

    // Pause Menu
    public void ShowPauseUI()
    {
        pauseMenu.SetActive(true);
    }

    public void HidePauseUI()
    {
        pauseMenu.SetActive(false);
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
    public void ShowHintUI(string hint)
    {
        hintBox.SetActive(true);
        hintText.text = hint;
    }

    public void HideHintUI()
    {
        hintBox.SetActive(false);
    }
}

