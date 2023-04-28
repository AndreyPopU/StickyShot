using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float density;
    public float growValue = .05f;
    public Transform gfx;

    public void Stick(Transform parent)
    {
        gfx.transform.SetParent(parent);
        Destroy(gameObject);
    }
}
