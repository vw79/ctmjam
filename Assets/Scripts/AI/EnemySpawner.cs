using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject slingshotPrefab;
    [SerializeField] private GameObject drillerPrefab;

    [SerializeField] private float enemyInterval = 4f;
    [SerializeField] private float slingshotInterval = 7f;
    [SerializeField] private float drillerInterval = 10f;

    [SerializeField] private int poolSize = 8;

    [SerializeField] private Transform playerTransform; // Reference to the player's transform
    [SerializeField] private float safeDistance = 1f; // Minimum safe distance from the player

    private List<GameObject> enemyPool;
    private List<GameObject> slingshotPool;
    private List<GameObject> drillerPool;

    void Start()
    {
        enemyPool = CreatePool(enemyPrefab, poolSize);
        slingshotPool = CreatePool(slingshotPrefab, poolSize);
        drillerPool = CreatePool(drillerPrefab, poolSize);

        StartCoroutine(SpawnFromPool(enemyInterval, enemyPool));
        StartCoroutine(SpawnFromPool(slingshotInterval, slingshotPool));
        StartCoroutine(SpawnFromPool(drillerInterval, drillerPool));
    }

    private List<GameObject> CreatePool(GameObject prefab, int size)
    {
        List<GameObject> pool = new List<GameObject>();
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
        return pool;
    }

    private IEnumerator SpawnFromPool(float interval, List<GameObject> pool)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            if (GameManager.instance.isDead)
            {
                yield return new WaitUntil(() => !GameManager.instance.isDead);
            }

            GameObject obj = GetPooledObject(pool);
            if (obj != null)
            {
                Vector3 spawnPosition;
                do
                {
                    spawnPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-6f, 6f), 0);
                } while (Vector3.Distance(spawnPosition, playerTransform.position) < safeDistance);

                obj.transform.position = spawnPosition;
                obj.SetActive(true);
            }
        }
    }

    private GameObject GetPooledObject(List<GameObject> pool)
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }
        return null;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}