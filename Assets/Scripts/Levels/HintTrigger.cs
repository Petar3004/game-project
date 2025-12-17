using UnityEngine;
using UnityEngine.SceneManagement;

public class HintTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        ManagersRoot.instance.hintManager.ShowHint(currentLevelIndex, transform.position);
    }
}
