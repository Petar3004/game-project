using UnityEngine;

public class LevelDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ManagersRoot.instance.audioManager.PlaySFX(ManagersRoot.instance.audioManager.levelComplete);
            ManagersRoot.instance.sceneController.GoToNextLevel();
        }
    }
}
