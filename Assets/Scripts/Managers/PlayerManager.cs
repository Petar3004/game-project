using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private GameObject currentPlayer;

    public void SpawnPlayer(Vector3 position)
    {
        if (currentPlayer != null)
            Destroy(currentPlayer);

        currentPlayer = Instantiate(playerPrefab, position, Quaternion.identity);
    }

    public GameObject Player => currentPlayer;
}
