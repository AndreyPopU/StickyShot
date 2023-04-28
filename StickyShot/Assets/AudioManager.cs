using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioClip buttonSound;
    public AudioClip stickSound;

    public Color activeColor;
    public bool musicMuted;
    public bool soundMuted;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayButton()
    {
        if (soundMuted) return;
        AudioSource.PlayClipAtPoint(buttonSound, transform.position, 1);
    }

    public void PlayStick()
    {
        if (soundMuted) return;

        AudioSource.PlayClipAtPoint(stickSound, transform.position, 1);
    }

    public void MuteMusic()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            Text buttonText = EventSystem.current.currentSelectedGameObject.GetComponent<Button>().GetComponentInChildren<Text>();

            if (!musicMuted) { buttonText.color = Color.white; musicMuted = true; }
            else { buttonText.color = activeColor; musicMuted = false; }
        }

        audioSource.mute = musicMuted;
    }

    public void MuteSound()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            Text buttonText = EventSystem.current.currentSelectedGameObject.GetComponent<Button>().GetComponentInChildren<Text>();

            if (!soundMuted) { buttonText.color = Color.white; soundMuted = true; }
            else { buttonText.color = activeColor; soundMuted = false; }
        }
    }
}
