using UnityEngine;
using static Enemy;

public class DrillerBullet : MonoBehaviour
{
    public float speed;
    private Vector3 direction;
    private int damage = 1;
    private PlayerLife playerLife;
    private EnemySpawner spawner; // Reference to the spawner to return the bullet

    void Start()
    {
        playerLife = GameObject.Find("Player").GetComponent<PlayerLife>();
        spawner = GameObject.FindObjectOfType<EnemySpawner>(); // Get the spawner reference
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;

        // Set the bullet's rotation to face the direction it's moving
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !GameManager.instance.isDead)
        {
            playerLife.TakeDamage(damage);
            spawner.ReturnToPool(gameObject); // Return the bullet to the pool
        }
        else if (collision.gameObject.CompareTag("Hitbox"))
        {
            spawner.ReturnToPool(gameObject);
        }
    }

    private void OnEnable()
    {
        Invoke("ReturnToPool", 3f); // Return the bullet to the pool after 3 seconds
    }

    private void OnDisable()
    {
        CancelInvoke("ReturnToPool"); // Cancel the invoke when the bullet is disabled
    }

    private void ReturnToPool()
    {
        spawner.ReturnToPool(gameObject); // Return the bullet to the pool
    }
}
