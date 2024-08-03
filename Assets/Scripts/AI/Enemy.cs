using System.Collections;
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
    [SerializeField] private int regularDamage = 1;
    [SerializeField] private int slingshotDamage = 2;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isDamaging = false;
    private bool isLunging = false;
    private PlayerLife playerLife;
    private Vector2 originalPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerLife = player.GetComponent<PlayerLife>();
        originalPosition = rb.position;
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
    }

    private void RegularEnemyBehavior(Vector3 direction)
    {
        if (direction.magnitude > stopDistance)
        {
            direction.Normalize();
            movement = direction;
            isDamaging = false;
        }
        else
        {
            movement = Vector2.zero;
            if (!isDamaging)
            {
                isDamaging = true;
                StartCoroutine(DamagePlayer(regularDamage, 1f));
            }
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

    private void FixedUpdate()
    {
        if (!isLunging)
        {
            moveEnemy(movement);
        }
    }

    void moveEnemy(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    private IEnumerator DamagePlayer(int damage, float damageInterval)
    {
        while (isDamaging)
        {
            if (playerLife != null)
            {
                playerLife.TakeDamage(damage);
            }
            Debug.Log("Player is being Damaged by " + enemyType + "!");
            yield return new WaitForSeconds(damageInterval);
        }
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

        Debug.Log("Player hit by Slingshot!");

        if (playerLife != null)
        {
            playerLife.TakeDamage(slingshotDamage);
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
}
