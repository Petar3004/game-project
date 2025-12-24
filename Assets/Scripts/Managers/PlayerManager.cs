using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector]
    public GameObject playerPrefab;
    private GameObject currentPlayer;

    // Map of levels and their spawn points
    public Dictionary<int, Vector3[]> spawnPoints = new Dictionary<int, Vector3[]>
    {
        { 1, new Vector3[] { new Vector3(-26f, -3.2f, 0f), new Vector3(-7.76f, -3.2f, 0f), new Vector3(27.83f, -3.2f, 0f) } },
        { 2, new Vector3[] { new Vector3(-11.5f, -4.1f, 0f), new Vector3(-24.68f, 5.78f, 0), new Vector3(-11.53f, 15.84f, 0) } },
        { 3, new Vector3[] { new Vector3(0.12f, -3.7f, 0) } },
        { 4, new Vector3[] { new Vector3(-7.8f, -3.75f, 0), new Vector3(10.28f, -3.43f, 0), new Vector3(11.2f, 5.8f, 0) } },
        { 5, new Vector3[] { new Vector3(-25.03f, -3f, 0f), new Vector3(47.62f, -0.36f, 0), new Vector3(57.66f, 17.35f, 0)} },
        { 6, new Vector3[] { new Vector3(27.13f, -3.43f, 0f) } },
        { 7, new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero } },
        { 8, new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero } },
        { 9, new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero } }
    };

    public void SpawnPlayer(int level, int room)
    {
        if (currentPlayer != null)
            Destroy(currentPlayer);

        currentPlayer = Instantiate(playerPrefab, spawnPoints[level][room], Quaternion.identity);
    }

    public void TurnRed()
    {
        currentPlayer.GetComponentInChildren<SpriteRenderer>().color = Color.red;
    }

    public GameObject Player => currentPlayer;
}
