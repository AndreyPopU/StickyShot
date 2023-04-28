using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public float shootForce; // upgradable
    public float maxLength; // upgradable
    public bool launching;
    public bool launched = false;

    [Header("Launching stuff")]
    public Transform player;
    public float bottomBoundary;
    public LineRenderer[] strips;
    public Transform[] stripPositions;
    public Transform origin;
    public Transform idlePos;
    public Vector3 currentPosition;

    private float jumpSuppresion = 1;
    private CameraManager cameraManager;
    private Camera cam;
    private Rigidbody2D playerRb;

    void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        cam = Camera.main;
        playerRb = player.GetComponent<Rigidbody2D>();

        strips[0].positionCount = 2;
        strips[1].positionCount = 2;
        strips[0].SetPosition(0, stripPositions[0].position);
        strips[1].SetPosition(0, stripPositions[1].position);

        ResetStrips();
    }

    void Update()
    {
        if (launched || GameManager.instance.upgradePanel.activeInHierarchy) return;

        if (launching)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;

            currentPosition = cam.ScreenToWorldPoint(mousePosition);
            currentPosition = origin.position + Vector3.ClampMagnitude(currentPosition - origin.position, maxLength);

            currentPosition = ClampBoundary(currentPosition);
            player.position = currentPosition;

            SetStrips(currentPosition);
        }
        else ResetStrips();
    }

    private void Shoot()
    {
        CalculateShootPower();
        playerRb.isKinematic = false;
        playerRb.velocity = Vector2.zero;
        player.GetComponent<Player>().density = GameManager.instance.ballDensity;
        player.GetComponent<CircleCollider2D>().enabled = true;

        Vector2 force = ((currentPosition - origin.position).normalized * shootForce * -1f) * Time.deltaTime;
        force = new Vector2(force.x, force.y / jumpSuppresion);
        playerRb.velocity = force;

        player.GetComponentInChildren<ParticleSystem>().Play();
        cameraManager.follow = true;
    }

    private void OnMouseDown()
    {
        if (launched || GameManager.instance.upgradePanel.activeInHierarchy) return;

        launching = true;
    }

    private void OnMouseUp()
    {
        if (launching)
        {
            launching = false;
            Shoot();
            launched = true;
            GameManager.instance.launched = true;
            ResetStrips();
        }
    }

    private void ResetStrips()
    {
        currentPosition = idlePos.position;
        SetStrips(currentPosition);
        if (!launched) player.position = currentPosition;
    }

    private void SetStrips(Vector3 position)
    {
        strips[0].SetPosition(1, position);
        strips[1].SetPosition(1, position);
    }

    private void CalculateShootPower()
    {
        switch (GameManager.instance.launcherLevel)
        {
            case 1: shootForce = 750; jumpSuppresion = 1; break;
            case 2: shootForce = 1500; jumpSuppresion = 1;  break;
            case 3: shootForce = 2250; jumpSuppresion = 1; break;
            case 4: shootForce = 3000; jumpSuppresion = 1.2f; break;
            case 5: shootForce = 3750; jumpSuppresion = 1.5f; break;
            case 6: shootForce = 4500; jumpSuppresion = 1.75f; break;
            case 7: shootForce = 5250; jumpSuppresion = 2f; break;
            case 8: shootForce = 6000; jumpSuppresion = 2.3f; break;
            case 9: shootForce = 7000; jumpSuppresion = 2.8f; break;
            case 10: shootForce = 11000; jumpSuppresion = 3.75f; break;
        }
    }

    private Vector3 ClampBoundary(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, -1000, transform.position.x);
        position.y = Mathf.Clamp(position.y, bottomBoundary, 1000);
        return position;
    }
}
