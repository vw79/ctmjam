using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject slingshot;

    [SerializeField] private float enemyInterval = 4f;
    [SerializeField] private float slingshotInterval = 7f;


     void Start()
    {
        StartCoroutine(spawnEnemies(enemyInterval, enemy));
        StartCoroutine(spawnEnemies(slingshotInterval, slingshot));
    }
    private IEnumerator spawnEnemies(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-5f, 5), Random.Range(-6f, 6f), 0), Quaternion.identity);
        StartCoroutine(spawnEnemies(interval, enemy));
    }
}
