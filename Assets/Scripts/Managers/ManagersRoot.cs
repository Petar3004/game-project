using UnityEngine;

public class ManagersRoot : MonoBehaviour
{
    public static ManagersRoot instance;

    [Header("Managers")]
    public GameManager gameManager;
    public SceneController sceneController;
    public TimeManager timeManager;
    public CameraController cameraController;
    public PauseManager pauseManager;
    public AbilityManager abilityManager;

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

        ManagersRoot prefab =
            Resources.Load<ManagersRoot>("Managers");

        if (prefab == null)
        {
            Debug.LogError("Managers prefab not found in Resources!");
            return;
        }

        Instantiate(prefab);
    }
}
