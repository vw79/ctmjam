using System.Collections;
using System.Collections.Generic;
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
            SoundManager.instance.Play("bgm");
        }
        else if (buildIndex == 1)
        {
            InitializeSceneTrans();
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
        gamerOverGO = GameObject.Find("GameOver");
        sceneTrasitionImage.enabled = true;
        gamerOverGO.SetActive(false);
    }

    private void Update()
    {
        if (isDead)
        {
            GameOver();
        }
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
    }
}