using UnityEditor.Callbacks;
using UnityEngine;

public class Scorpion : MonoBehaviour
{
    public Rigidbody2D scorpionRb;
    public float speed = 5f;
    private bool movingRight = false;
    private float elapsed;


    void Update()
    {
        if (ManagersRoot.instance.playerManager.Player == null)
        {
            return;
        }

        if (ManagersRoot.instance.playerManager.Player.transform.position.y < -3f)
        {
            Chase();
        }
        else
        {
            Roam();
        }
    }

    private void Roam()
    {
        if (elapsed >= 5)
        {
            float randFloat = Random.Range(0f, 1f);
            if (randFloat > 0.8f)
            {
                movingRight = !movingRight;
            }
            elapsed = 0;
        }
        elapsed += Time.deltaTime;

        if (movingRight)
        {
            scorpionRb.linearVelocityX = speed / 2;
        }
        else
        {
            scorpionRb.linearVelocityX = -speed / 2;
        }
    }

    private void Chase()
    {
        if (ManagersRoot.instance.playerManager.Player.transform.position.x < transform.position.x)
        {
            movingRight = false;
            scorpionRb.linearVelocityX = -speed;
        }
        else
        {
            movingRight = true;
            scorpionRb.linearVelocityX = speed;
        }
    }
}
