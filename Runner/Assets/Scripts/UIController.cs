using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    int distance;
    Player player;
    TextMeshProUGUI distanceText;

    GameObject results;
    TextMeshProUGUI finalDistanceText;

    public TextMeshProUGUI highScoreText;

    public InterstitialAds ad;

    public GameObject musicObj;
    public GameObject ambienceObj;

    public GameObject pauseMenuUI;
    public GameObject pauseButtonUI;
    public GameObject resumeButtonUI;
    public static bool gameIsPaused = false;



    private void Awake()
    {

        player = GameObject.Find("Player").GetComponent<Player>();
        distanceText = GameObject.Find("DistanceText").GetComponent<TextMeshProUGUI>();

        musicObj = GameObject.Find("Music");
        ambienceObj = GameObject.Find("Ambience");

        finalDistanceText = GameObject.Find("FinalDistanceText").GetComponent<TextMeshProUGUI>();
        results = GameObject.Find("Results");
        results.SetActive(false);
    }


    void Start()
    {
        Application.targetFrameRate = 120;
    }

    void Update()
    {
        distance = Mathf.FloorToInt(player.distance);
        distanceText.text = distance + "m";
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("highscore") + "m";

        if (player.isDead)
        {
            results.SetActive(true);
            pauseButtonUI.SetActive(false);
            resumeButtonUI.SetActive(false);
            player.velocity.x = 0;
            finalDistanceText.text = distance + "m";
            if(distance > PlayerPrefs.GetInt("highscore"))
            {
                PlayerPrefs.SetInt("highscore", distance);
            }
        }

    }

    public void Quit()
    {

        if (musicObj != null)
        {
            Destroy(musicObj);
        }

        if (ambienceObj != null)
        {
            Destroy(ambienceObj);
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
        
    }

    public void Retry()
    {
        PlayerPrefs.SetInt("tempAds", PlayerPrefs.GetInt("tempAds") + 1);
        Debug.Log(PlayerPrefs.GetInt("tempAds"));
        

        if (PlayerPrefs.GetInt("tempAds") >= 3)
        {
            ad.ShowAd();
            Time.timeScale = 0;
            MuteAllSound();
            PlayerPrefs.SetInt("tempAds", 0);
        }
        Debug.Log(PlayerPrefs.GetInt("tempAds"));
        SceneManager.LoadScene("SampleScene");


    }

    public void RetryPause()
    {
        PlayerPrefs.SetInt("tempAds", PlayerPrefs.GetInt("tempAds") + 1);
        Debug.Log(PlayerPrefs.GetInt("tempAds"));


        if (PlayerPrefs.GetInt("tempAds") >= 3)
        {
            ad.ShowAd();
            Time.timeScale = 0;
            MuteAllSound();
            PlayerPrefs.SetInt("tempAds", 0);
        }
        else
        {
            Time.timeScale = 1f;
        }
        Debug.Log(PlayerPrefs.GetInt("tempAds"));
        SceneManager.LoadScene("SampleScene");


    }

    public void PauseMenu()
    {
        pauseMenuUI.SetActive(true);
        pauseButtonUI.SetActive(false);
        resumeButtonUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;

    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        pauseButtonUI.SetActive(true);
        resumeButtonUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void MuteAllSound()
    {
        AudioListener.volume = 0;
    }

    public void UnMuteAllSound()
    {
        AudioListener.volume = 1;
    }


}
