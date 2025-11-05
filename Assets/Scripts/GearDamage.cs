using UnityEngine;

public class GearDamage : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public int damage = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D player)
    {
        if (player.gameObject.tag == "PlayerObject")
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
