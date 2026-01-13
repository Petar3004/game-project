using System.Collections;
using TMPro;
using Unity.VisualScripting;
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
    public Canvas HUD;

    [Header("Timer")]
    public Slider timerSlider;
    public Image timerFill;
    public float flashSpeed;
    private Coroutine flashCoroutine;

    [Header("Health")]
    public Image heart1;
    public Image heart2;
    public Image heart3;

    [Header("Abilities")]
    public Image abilityKey;
    public Image abilityImage;

    [Header("Hints")]
    public Canvas overlay;
    public GameObject bigHintBox;
    public Image bigHintImage;
    public TMP_Text bigHintText;
    public GameObject smallHintBox;
    public Image smallHintImage;
    public TMP_Text smallHintText;

    [Header("Pause Menu")]
    public Canvas pauseMenu;

    [Header("Transitions")]
    public Canvas transition;
    public Image sceneFadeImage;

    [Header("Cutsene")]
    public Canvas cutscene;
    public TMP_Text skipText;
    public TMP_Text continueText;

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
        ActivateUI();
    }

    public void ActivateUI()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        bool isCutscene = currentLevelIndex > 9;
        bool isMainMenu = currentLevelIndex == 0;

        HUD.gameObject.SetActive(!isMainMenu && !isCutscene);
        overlay.gameObject.SetActive(!isMainMenu && !isCutscene);
        HideContinueText();
        HideSkipCutsceneText();
        cutscene.gameObject.SetActive(isCutscene);
        pauseMenu.gameObject.SetActive(false);
        bigHintBox.SetActive(false);
        smallHintBox.SetActive(false);
    }

    private Color ChangeAlpha(Image img, float alpha)
    {
        Color col = img.color;
        return new Color(col.r, col.g, col.b, alpha);
    }

    // Pause Menu
    public void ShowPauseUI()
    {
        pauseMenu.gameObject.SetActive(true);
    }

    public void HidePauseUI()
    {
        if (pauseMenu.gameObject.activeInHierarchy)
        {
            pauseMenu.gameObject.SetActive(false);
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
        bool timerFlashing = ManagersRoot.instance.timeManager.timeLeft < ManagersRoot.instance.abilityManager.abilityTimePenalty;
        timerSlider.value = timeLeft;

        if (timerFlashing && flashCoroutine == null)
        {
            flashCoroutine = StartCoroutine(FlashSlider());
        }
        else if (!timerFlashing && flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
            timerFill.color = Color.white;
        }
    }

    public void UpdateMaxTimerUI(float value)
    {
        timerSlider.maxValue = value;
    }

    private IEnumerator FlashSlider()
    {
        while (true)
        {
            float t = Mathf.PingPong(Time.unscaledTime * flashSpeed, 1f);
            timerFill.color = Color.Lerp(Color.white, Color.yellow, t);
            yield return null;
        }
    }

    // Health
    public void UpdateHealthUI()
    {
        switch (ManagersRoot.instance.playerManager.Player.GetComponent<PlayerHealth>().currentHealth)
        {
            case 2:
                heart3.gameObject.SetActive(false);
                break;
            case 1:
                heart3.gameObject.SetActive(false);
                heart2.gameObject.SetActive(false);
                break;
            case 0:
                heart3.gameObject.SetActive(false);
                heart2.gameObject.SetActive(false);
                heart1.gameObject.SetActive(false);
                break;
            default:
                heart3.gameObject.SetActive(true);
                heart2.gameObject.SetActive(true);
                heart1.gameObject.SetActive(true);
                break;
        }
    }


    // Abilities
    public void UpdateAbiliyUI()
    {
        AbilityType currentAbility = ManagersRoot.instance.abilityManager.ability;
        abilityImage.sprite = ManagersRoot.instance.abilityManager.abilityToData[currentAbility].Item2;
        abilityImage.fillAmount = ManagersRoot.instance.abilityManager.abilityCharge;
        if (ManagersRoot.instance.abilityManager.abilityIsActive || ManagersRoot.instance.timeManager.timeLeft < ManagersRoot.instance.abilityManager.abilityTimePenalty)
        {
            abilityKey.color = ChangeAlpha(abilityKey, 0.2f);
        }
        else
        {
            abilityKey.color = ChangeAlpha(abilityKey, 1f);
        }
        if (ManagersRoot.instance.timeManager.timeLeft < ManagersRoot.instance.abilityManager.abilityTimePenalty)
        {
            abilityImage.color = Color.softRed;
        }
        else
        {
            abilityImage.color = Color.white;
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

    // Fading 
    public IEnumerator FadeInCoroutine(float duration)
    {
        Color startColor = new Color(sceneFadeImage.color.r, sceneFadeImage.color.g, sceneFadeImage.color.b, 1);
        Color targetColor = new Color(sceneFadeImage.color.r, sceneFadeImage.color.g, sceneFadeImage.color.b, 0);

        yield return FadeCoroutine(startColor, targetColor, duration);

        transition.gameObject.SetActive(false);
    }

    public IEnumerator FadeOutCoroutine(float duration)
    {
        Color startColor = new Color(sceneFadeImage.color.r, sceneFadeImage.color.g, sceneFadeImage.color.b, 0);
        Color targetColor = new Color(sceneFadeImage.color.r, sceneFadeImage.color.g, sceneFadeImage.color.b, 1);

        transition.gameObject.SetActive(true);
        yield return FadeCoroutine(startColor, targetColor, duration);
    }

    private IEnumerator FadeCoroutine(Color startColor, Color targetColor, float duration)
    {
        float elapsedTime = 0;
        float elapsedPercentage = 0;

        while (elapsedPercentage < 1)
        {
            elapsedPercentage = elapsedTime / duration;
            sceneFadeImage.color = Color.Lerp(startColor, targetColor, elapsedPercentage);

            yield return null;
            elapsedTime += Time.unscaledDeltaTime;
        }
    }

    // Cutscene
    public void ShowSkipCutsceneText()
    {
        skipText.gameObject.SetActive(true);
    }

    public void HideSkipCutsceneText()
    {
        skipText.gameObject.SetActive(false);
    }

    public void ShowContinueText()
    {
        continueText.gameObject.SetActive(true);
    }

    public void HideContinueText()
    {
        continueText.gameObject.SetActive(false);
    }
}

