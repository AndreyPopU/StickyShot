using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float ballSize;
    [Tooltip("Percentage reduce")]
    public float density;
    public bool invincible;
    public float resetTime = 2;
    public Transform gfx;

    private ParticleSystem trail;
    private CircleCollider2D coreCollider;
    private Rigidbody2D rb;
    private float trialSizeMax;
    private float trialSizeMin;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        trail = GetComponentInChildren<ParticleSystem>();
        coreCollider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        var main = trail.main;
        trialSizeMin = main.startSize.constantMin;
        trialSizeMax = main.startSize.constantMax;
    }

    private void Update()
    {
        ResetController();

        if (GameManager.instance.launched)
        {
            GameManager.instance.UpdateMeterCounter(Mathf.RoundToInt(transform.position.x));
            if (rb.velocity.x > .2f) rb.velocity = new Vector2(rb.velocity.x - .0009f, rb.velocity.y);
        }
    }

    private void ResetController()
    {
        if (!GameManager.instance.launched) return;

        if (rb.velocity.x < .2f)
        {
            if (resetTime > 0) resetTime -= Time.deltaTime;
            else
            {
                GameManager.instance.EndGame();
            }
        }
        else { if (resetTime < 2) resetTime = 2; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Obstacle obstacle))
        {
            obstacle.Stick(transform);
            ballSize += obstacle.growValue;

            // Remove velocity based on obstacle density
            rb.velocity = new Vector2(rb.velocity.x - obstacle.density / density, rb.velocity.y);
            if (rb.velocity.x < 0) rb.velocity = new Vector2(0, rb.velocity.y);

            coreCollider.radius = ballSize;
            cam.orthographicSize += obstacle.growValue / 1.5f;
            gfx.transform.localScale = Vector3.one * ballSize;

            if (ballSize < 3.5f)
            {
                var main = trail.main;
                main.startSize = new ParticleSystem.MinMaxCurve(trialSizeMin * (ballSize * 1.25f), trialSizeMax * (ballSize * 1.25f) * 1.2f);
            }

            AudioManager.instance.PlayStick();
            var shape = trail.shape;
            shape.scale = gfx.localScale;
            density += obstacle.density / 160;
        }

        if (collision.GetComponent<PowerUp>()) collision.GetComponent<PowerUp>().Use();
    }
}
