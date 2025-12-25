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

    [Header("Abilities")]
    public TMP_Text abilityText;
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
        bool isMainMenu = SceneManager.GetActiveScene().buildIndex == 0;

        HUD.gameObject.SetActive(!isMainMenu);
        overlay.gameObject.SetActive(!isMainMenu);
        pauseMenu.gameObject.SetActive(false);
        bigHintBox.SetActive(false);
        smallHintBox.SetActive(false);
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

    private IEnumerator FlashSlider()
    {
        while (true)
        {
            float t = Mathf.PingPong(Time.unscaledTime * flashSpeed, 1f);
            timerFill.color = Color.Lerp(Color.white, Color.yellow, t);
            yield return null;
        }
    }


    // Abilities
    public void UpdateAbiliyUI()
    {
        AbilityType currentAbility = ManagersRoot.instance.abilityManager.ability;
        abilityImage.fillAmount = ManagersRoot.instance.abilityManager.abilityCharge;
        abilityText.text = ManagersRoot.instance.abilityManager.abilityToData[currentAbility].Item1;
        if (ManagersRoot.instance.timeManager.timeLeft < ManagersRoot.instance.abilityManager.abilityTimePenalty)
        {
            abilityImage.color = Color.softRed;
        }
        else
        {
            abilityImage.color = ManagersRoot.instance.abilityManager.abilityToData[currentAbility].Item2;
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
}

