using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public TrailRenderer ballTrail;


    public int time = 7;
    public bool isFinished;
    public bool isGameOver = false;

    // create array of ground pieces
    public GroundPiece[] allGroundPieces;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void SetupNewLevel()
    {
        allGroundPieces = FindObjectsOfType<GroundPiece>();
        if (ballTrail != null )
        {
            ballTrail.enabled = true;
        }
    }

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else if (singleton != this)
        {
            if (ballTrail != null)
            {
                ballTrail.enabled = false;
            }
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

        UpdateTime();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        SetupNewLevel();
    }

    public void CheckComplete()
    {
        isFinished = true;

        for (int i = 0; i < allGroundPieces.Length; i++)
        {
            if (allGroundPieces[i].isColored == false)
            {
                if (time <= 0)
                {
                    GameOver();
                    return;
                }
                else
                {
                    isFinished = false;
                    break;
                }
            }
        }

        if (isFinished)
        {
            NextLevel();
        }
    }

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        isGameOver = true;
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(0);
    }

    private void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    IEnumerator CountDown()
    {
        while (!isFinished && time >= 0)
        {
            timeText.text = "time: " + time;
            Debug.Log("Timer text updated: " + timeText.text);
            yield return new WaitForSeconds(1);
            time--;
        }

        GameOver();
    }

    public void UpdateTime()
    {
        StartCoroutine(CountDown());
    }
}