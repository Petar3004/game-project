using UnityEngine;

public enum ButtonType
{
    LEFT,
    RIGHT
}

public class ClockControlButton : MonoBehaviour
{
    public SandClock sandClock;
    public ButtonType buttonType;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerObject"))
        {
            sandClock.SpinClock(buttonType);
        }
    }
}
