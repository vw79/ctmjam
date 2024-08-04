using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType { Regular, Slingshot }
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float stopDistance = 2f;
    [SerializeField] private float lungeSpeed = 10f;
    [SerializeField] private float slingshotCooldown = 2f;
    [SerializeField] private int regularDamage;
    [SerializeField] private int slingshotDamage;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isLunging = false;
    private PlayerLife playerLife;
    private Vector2 originalPosition;
    private EnemySpawner spawner;

    void Start()
    {
        player = GameObject.Find("PlayerSprite").GetComponent<Transform>(); 
        rb = GetComponent<Rigidbody2D>();
        playerLife = GameObject.Find("Player").GetComponent<PlayerLife>();
        originalPosition = rb.position;
        spawner = GameObject.FindObjectOfType<EnemySpawner>();
    }

    void Update()
    {
        Vector3 direction = player.position - transform.position;

        if (enemyType == EnemyType.Regular)
        {
            RegularEnemyBehavior(direction);
        }
        else if (enemyType == EnemyType.Slingshot)
        {
            SlingshotEnemyBehavior(direction);
        }

        if (!isLunging)
        {
            moveEnemy(movement);
        }
    }

    private void RegularEnemyBehavior(Vector3 direction)
    {
        if (direction.magnitude > stopDistance)
        {
            direction.Normalize();
            movement = direction;
        }
        else
        {
            movement = Vector2.zero;
        }
    }

    private void SlingshotEnemyBehavior(Vector3 direction)
    {
        if (!isLunging)
        {
            if (direction.magnitude > stopDistance)
            {
                direction.Normalize();
                movement = direction;
            }
            else
            {
                movement = Vector2.zero;
                StartCoroutine(Lunge());
            }
        }
    }

    void moveEnemy(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    private IEnumerator Lunge()
    {
        isLunging = true;

        Vector2 lungeTarget = (Vector2)player.position;

        // Lunge towards the player
        while (Vector2.Distance(rb.position, lungeTarget) > 0.1f)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, lungeTarget, lungeSpeed * Time.deltaTime));
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        // Retreat back to the original position
        while (Vector2.Distance(rb.position, originalPosition) > 0.1f)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, originalPosition, lungeSpeed * Time.deltaTime));
            yield return null;
        }

        yield return new WaitForSeconds(slingshotCooldown);

        isLunging = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (enemyType == EnemyType.Regular)
            {
                playerLife.TakeDamage(regularDamage);
            }
            else if (enemyType == EnemyType.Slingshot)
            {
                playerLife.TakeDamage(slingshotDamage);
            }
        }
        else if (collision.gameObject.CompareTag("Hitbox"))
        {
            spawner.ReturnToPool(gameObject);
        }
    }
}