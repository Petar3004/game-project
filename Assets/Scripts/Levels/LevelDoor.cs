using UnityEngine;

public class LevelDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            ManagersRoot.instance.gameManager.MovePlayerToNextLevel();
        }
    }
}
