using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { Boost, Invincible }
    public PowerUpType type;
    public int boostLevel;
    [Header("Boost")]
    public float force;
    [Header("Invincible")]
    public float minSpeed; // if the player speed is bellow that set it to that, if it isn't don't touch it, don't affect player by density
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();

        //int random = Random.Range(0, 4);
        //if (random == 1) type = PowerUpType.Invincible;
    }

    public void Use()
    {
        if (type == PowerUpType.Boost)
        {
            player.GetComponent<Rigidbody2D>().AddForce(Vector3.right * force * GameManager.instance.powerUpLevel * Time.deltaTime, ForceMode2D.Impulse);
            Destroy(gameObject);
        }
        else if (type == PowerUpType.Invincible)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(Invincible(10 * GameManager.instance.powerUpLevel / 2, minSpeed * GameManager.instance.powerUpLevel / 1.5f));
        }
    }

    public IEnumerator Invincible(float time, float minSpeed)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        float playerXSpeed = rb.velocity.x;
        player.invincible = true;

        while (time > 0)
        {
            time -= Time.deltaTime;
            if (playerXSpeed < minSpeed) rb.velocity = new Vector2(minSpeed, rb.velocity.y);
            else rb.velocity = new Vector2(playerXSpeed, rb.velocity.y);

            yield return null;
        }

        player.invincible = false;
        Destroy(gameObject);
    }
}
