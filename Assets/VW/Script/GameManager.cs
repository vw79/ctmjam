using System.Collections;
using System.Collections.Generic;
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
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        if (buildIndex == 0)
        {
            InitializeSceneTrans();
            InitializeGameElement();
            ResetTimer();
            SoundManager.instance.Play("bgm");
        }
        else if (buildIndex == 1)
        {
            InitializeSceneTrans();
            SoundManager.instance.Play("bgm");
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

        ResetTimer();
    }

    private void ResetTimer()
    {
        elapsedTime = 0;
        isTimerRunning = true;
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
        endKillText.text = "0";
        endGoldText.text = "0";
    }
}