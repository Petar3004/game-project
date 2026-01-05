using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SandClockPuzzle : MonoBehaviour
{
    private int numPieces = 0;
    public int maxNumPieces = 3;
    public TMP_Text numPiecesUI;
    public TMP_Text sequenceTextUI;
    public List<SandClock> clocks;

    void Start()
    {
        sequenceTextUI.gameObject.SetActive(false);
        UpdatePiecesUI(0);
    }

    void Update()
    {
        if (IsComplete())
        {
            ManagersRoot.instance.sceneController.GoToMainMenu();
            ManagersRoot.instance.gameManager.chapterComplete = true;
        }
    }

    public void GetSequencePiece()
    {
        numPieces++;
        if (numPieces == maxNumPieces)
        {
            sequenceTextUI.gameObject.SetActive(true);
            numPiecesUI.gameObject.SetActive(false);
        }

        UpdatePiecesUI(numPieces);
    }

    private void UpdatePiecesUI(int numPieces)
    {
        numPiecesUI.text = numPieces + "/" + maxNumPieces;
    }

    private bool IsComplete()
    {
        foreach (SandClock clock in clocks)
        {
            if (!clock.completed)
            {
                return false;
            }
        }
        return true;
    }
}
