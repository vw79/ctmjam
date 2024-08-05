using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private Animator animator;
    private Image sceneTransitionImage;
    private GameObject gameOverGO;
    public bool isDead = false;
    private bool isGameOverHandled = false;

    private TextMeshProUGUI timerText;
    private TextMeshProUGUI coinText;

    private float elapsedTime;
    private bool isTimerRunning;

    private TextMeshProUGUI endTimeText;
    private TextMeshProUGUI endKillText;
    private TextMeshProUGUI endGoldText;

    private int enemiesKilled;
    private int coinsCollected;

    public GameData gameData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeSceneTrans();

        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        if (buildIndex == 0)
        {
            SoundManager.instance.Play("bgm");
        }
        else if (buildIndex == 1)
        {
            InitializeGameElement();
            ResetGame();
            SoundManager.instance.Play("bgm");
        }
        else if (buildIndex == 3)
        {
            coinText = GameObject.Find("coinText").GetComponent<TextMeshProUGUI>();
            if (coinText != null)
            {
                coinText.text = "Coins: " + gameData.totalCoinsCollected.ToString();
            }
        }
        animator.SetTrigger("Start");
        StartCoroutine(DisableSceneTransitionCoroutine());
    }

    private void InitializeSceneTrans()
    {
        isDead = false;
        isGameOverHandled = false;

        animator = GameObject.Find("SceneTrans").GetComponentInChildren<Animator>();
        sceneTransitionImage = GameObject.Find("SceneTrans").GetComponentInChildren<Image>();
    }

    private void InitializeGameElement()
    {
        isDead = false;
        isGameOverHandled = false;
        gameOverGO = GameObject.Find("Canvas/GameOver");
        sceneTransitionImage = GameObject.Find("SceneTrans").GetComponentInChildren<Image>();
        sceneTransitionImage.enabled = true;
        gameOverGO.SetActive(false);

        timerText = GameObject.Find("Timer").GetComponentInChildren<TextMeshProUGUI>();

        Transform gameOverTransform = gameOverGO.transform;
        endTimeText = gameOverTransform.Find("Total Time").GetComponent<TextMeshProUGUI>();
        endKillText = gameOverTransform.Find("Enemies Killed").GetComponent<TextMeshProUGUI>();
        endGoldText = gameOverTransform.Find("Coins Collected").GetComponent<TextMeshProUGUI>();

        ResetGame();
    }

    private void ResetGame()
    {
        elapsedTime = 0;
        isTimerRunning = true;
        enemiesKilled = 0;
        coinsCollected = 0;
        UpdateTimerText();
        UpdateCoinText();
    }

    private void Update()
    {
        if (isDead && !isGameOverHandled)
        {
            GameOver();
        }
        else if (isTimerRunning)
        {
            UpdateTimer();
        }
    }

    private void UpdateTimer()
    {
        elapsedTime += Time.deltaTime;
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    IEnumerator DisableSceneTransitionCoroutine()
    {
        yield return new WaitForSeconds(1f);
        sceneTransitionImage.enabled = false;
    }

    private void GameOver()
    {
        gameOverGO.SetActive(true);
        isTimerRunning = false;
        UpdateEndMenuText();
        isGameOverHandled = true;

        gameData.totalCoinsCollected += coinsCollected;
    }

    private void UpdateEndMenuText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        endTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        endKillText.text = enemiesKilled.ToString();
        endGoldText.text = coinsCollected.ToString();
    }

    public void AddCoin()
    {
        coinsCollected++;
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + coinsCollected.ToString();
        }
        if (endGoldText != null)
        {
            endGoldText.text = coinsCollected.ToString();
        }
    }

    public void AddEnemyKill()
    {
        enemiesKilled++;
        endKillText.text = enemiesKilled.ToString();
    }
}