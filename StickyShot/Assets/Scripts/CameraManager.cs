using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public bool follow;
    public Transform target;
    public float smoothness = .125f;
    public Vector3 offset = new Vector3(0, 0, -10);

    private void FixedUpdate()
    {
        if (!follow) return;

        transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothness);
    }

    public IEnumerator Shake(float duration, float force)
    {
        Vector3 originalPos = transform.position;
        Vector3 shakePos;

        while (duration > 0)
        {
            duration -= Time.deltaTime;
            float randomX = Random.Range(originalPos.x - 1 * force, originalPos.x + 1 * force);
            float randomY = Random.Range(originalPos.y - 1 * force, originalPos.y + 1 * force);
            shakePos = new Vector3(randomX, randomY, transform.position.z);
            transform.position = shakePos;
            yield return null;
        }

        transform.position = originalPos;
    }
}
