using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool finishedGame;
    public GameObject finishPanel;
    public int launcherLevel = 1;
    public int ballDensity = 1;
    public int powerUpLevel = 1;
    public Text meterText;
    public bool launched;
    public int coins;
    public Texture2D cursorSprite;
    [Header("UI Stuff")]
    public GameObject upgradePanel;
    public GameObject endGamePanel;
    public CanvasGroup fadePanel;
    public Text coinsText;
    public Text distanceText;
    public Text moneyText;
    [Header("Launcher")]
    public int launcherPrice;
    public Slider launcherSlider;
    public int[] launcherPrices;
    public Text launcherPriceText;
    [Header("Ball")]
    public int ballPrice;
    public Slider ballSlider;
    public int[] ballPrices;
    public Text ballPriceText;
    [Header("Power up")]
    public int powerUpPrice;
    public Slider powerUpSlider;
    public int[] powerUpPrices;
    public Text powerUpPriceText;

    private void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);

        StartCoroutine(Fade(0));

        Vector2 hotSpot = new Vector2(cursorSprite.width / 2f, cursorSprite.height / 2f);
        Cursor.SetCursor(cursorSprite, hotSpot, CursorMode.Auto);
    }

    void Start()
    {
        launcherPrice = launcherPrices[launcherLevel - 1];
        ballPrice = ballPrices[ballDensity - 1];
        powerUpPrice = powerUpPrices[powerUpLevel - 1];
        launcherPriceText.text = launcherPrice.ToString() + "$";
        ballPriceText.text = ballPrice.ToString() + "$";
        powerUpPriceText.text = powerUpPrice.ToString() + "$";
    }

    void Update()
    {

    }

    public void UpdateMeterCounter(float meters)
    {
        meterText.text = meters.ToString() + "m";
    }

    public void EndGame()
    {
        if (finishedGame || !launched) return;

        endGamePanel.SetActive(true);
        int money = Mathf.RoundToInt(FindObjectOfType<Player>().transform.position.x) * 2;
        coins += money;
        coinsText.text = coins.ToString() + "$";
        moneyText.text = "Money earned: " + money + "$";
        distanceText.text = "Distance traveled: " + meterText.text;
        meterText.text = "0m";

        launched = false;
    }

    public void ResetGame()
    {
        endGamePanel.SetActive(false);
        StartCoroutine(Fade(1));
    }

    public IEnumerator Fade(int desire)
    {
        if (fadePanel.alpha > desire)
        {
            while (fadePanel.alpha > desire)
            {
                fadePanel.alpha -= Time.deltaTime * 2;

                yield return null;
            }
        }
        else
        if (fadePanel.alpha < desire)
        {
            while (fadePanel.alpha < desire)
            {
                fadePanel.alpha += Time.deltaTime * 2;

                yield return null;
            }
        }

        if (fadePanel.alpha == 1)
        {
            if (!finishedGame)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                StartCoroutine(Fade(0));
                upgradePanel.SetActive(true);
            }
            else
            {
                finishPanel.SetActive(true);
                fadePanel.blocksRaycasts = true;
            }
        }
    }

    public void Buy(int index)
    {
        switch (index)
        {
            case 0: // Launcher
                if (coins >= launcherPrice)
                {
                    coins -= launcherPrice;
                    coinsText.text = coins.ToString() + "$";
                    launcherSlider.value++;
                    launcherLevel = (int)launcherSlider.value;
                    launcherPrice = launcherPrices[launcherLevel - 1];
                    launcherPriceText.text = launcherPrice.ToString() + "$";
                    if (launcherLevel == 10)
                    {
                        launcherPriceText.gameObject.SetActive(false);
                        EventSystem.current.currentSelectedGameObject.GetComponent<MyButton>().enabled = false;
                        EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;
                    }

                }
                break;
            case 1: // Ball
                if (coins >= ballPrice)
                {
                    coins -= ballPrice;
                    coinsText.text = coins.ToString() + "$";
                    ballSlider.value++;
                    ballDensity = (int)ballSlider.value;
                    ballPrice = ballPrices[ballDensity - 1];
                    ballPriceText.text = ballPrice.ToString() + "$";
                    if (ballDensity == 10)
                    {
                        ballPriceText.gameObject.SetActive(false);
                        EventSystem.current.currentSelectedGameObject.GetComponent<MyButton>().enabled = false;
                        EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;
                    }
                }
                break;
            case 2: // Power up
                if (coins >= powerUpPrice)
                {
                    coins -= powerUpPrice;
                    coinsText.text = coins.ToString() + "$";
                    powerUpSlider.value++;
                    powerUpLevel = (int)powerUpSlider.value;
                    powerUpPrice = powerUpPrices[powerUpLevel - 1];
                    powerUpPriceText.text = powerUpPrice.ToString() + "$";
                    if (powerUpLevel == 5)
                    {
                        powerUpPriceText.gameObject.SetActive(false);
                        EventSystem.current.currentSelectedGameObject.GetComponent<MyButton>().enabled = false;
                        EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;
                    }
                }
                break;
        }
    }

    public void Music()
    {
        if (!AudioManager.instance.musicMuted) AudioManager.instance.musicMuted = true;
        else AudioManager.instance.musicMuted = false;

        AudioManager.instance.GetComponent<AudioSource>().mute = AudioManager.instance.musicMuted;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
