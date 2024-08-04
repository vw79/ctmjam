using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private Animator animator;
    private Image sceneTrasitionImage;
    private GameObject gamerOverGO;
    public bool isDead = false;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI coinText; // Assign this in the inspector

    private float elapsedTime;
    private bool isTimerRunning;

    private TextMeshProUGUI endTimeText;
    private TextMeshProUGUI endKillText;
    private TextMeshProUGUI endGoldText;

    private int enemiesKilled;
    private int coinsCollected;

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
            // Display the amount of coins collected
            coinText = GameObject.Find("coinText").GetComponent<TextMeshProUGUI>();
            if (coinText != null)
            {
                coinsCollected = PlayerPrefs.GetInt("CoinsCollected", 0);
                coinText.text = "Coins: " + coinsCollected.ToString();
            }
        }
        animator.SetTrigger("Start");
        StartCoroutine(DisableSceneTransitionCoroutine());
    }

    private void InitializeSceneTrans()
    {
        isDead = false;

        animator = GameObject.Find("SceneTrans").GetComponentInChildren<Animator>();
        sceneTrasitionImage = GameObject.Find("SceneTrans").GetComponentInChildren<Image>();
    }

    private void InitializeGameElement()
    {
        isDead = false;
        gamerOverGO = GameObject.Find("Canvas/GameOver");
        sceneTrasitionImage = GameObject.Find("SceneTrans").GetComponentInChildren<Image>();
        sceneTrasitionImage.enabled = true;
        gamerOverGO.SetActive(false);

        timerText = GameObject.Find("Timer").GetComponentInChildren<TextMeshProUGUI>();

        Transform gameOverTransform = gamerOverGO.transform;
        endTimeText = gameOverTransform.Find("Total Time").GetComponent<TextMeshProUGUI>();
        endKillText = gameOverTransform.Find("Enemies Killed").GetComponent<TextMeshProUGUI>();
        endGoldText = gameOverTransform.Find("Coins Collected").GetComponent<TextMeshProUGUI>();

        // Retrieve coins collected from PlayerPrefs
        coinsCollected = PlayerPrefs.GetInt("CoinsCollected", 0);

        ResetGame();
    }

    private void ResetGame()
    {
        elapsedTime = 0;
        isTimerRunning = true;
        enemiesKilled = 0;
        UpdateTimerText();
    }

    private void Update()
    {
        if (isDead)
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
        sceneTrasitionImage.enabled = false;
    }

    private void GameOver()
    {
        gamerOverGO.SetActive(true);
        isTimerRunning = false;
        UpdateEndMenuText();
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
        endGoldText.text = coinsCollected.ToString();
        // Store coins collected in PlayerPrefs
        PlayerPrefs.SetInt("CoinsCollected", coinsCollected);
        PlayerPrefs.Save();
    }

    public void AddEnemyKill()
    {
        enemiesKilled++;
        endKillText.text = enemiesKilled.ToString();
    }
}