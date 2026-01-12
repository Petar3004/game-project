using UnityEngine;

public class DigitalClockButton : MonoBehaviour
{
    public DigitalClock clock;
    public int index;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerObject"))
        {
            clock.IncrementDigit(index);
        }
    }
}
