using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Animator animator;
    public GameObject sceneTransitionGO;
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
        animator.SetTrigger("Start");
        StartCoroutine(DisableSceneTransitionCoroutine());

        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        if (buildIndex == 0)
        {
            SoundManager.instance.Play("bgm");
        }
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    IEnumerator DisableSceneTransitionCoroutine()
    {
        yield return new WaitForSeconds(1f);
        sceneTransitionGO.SetActive(false);
    }
}