using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            print("herer");
            GameManager.instance.finishedGame = true;
            GameManager.instance.StartCoroutine(GameManager.instance.Fade(1));
        }
    }
}
