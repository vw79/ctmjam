using System.Collections;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float stopDistance = 4f; // Distance at which the enemy stops moving
    [SerializeField] private float lungeSpeed = 40f; // Speed of the lunge
    [SerializeField] private float retreatDistance = 4f; // Distance the enemy retreats after lunging
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isLunging = false;
    private bool isRetreating = false;

     void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

     void Update()
    {
        Vector3 direction = player.position - transform.position;

        if (!isLunging && !isRetreating)
        {
            if(direction.magnitude > stopDistance) 
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
        if (!isLunging && !isRetreating)
        {
            moveEnemy(movement);
        }
    }
    void moveEnemy(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    private IEnumerator Lunge()
    {
        isLunging = true;

        Vector2 originalPos = rb.position;
        Vector2 lungeTarget = (Vector2)player.position;

        //Lunge towards player
        while(Vector2.Distance(rb.position, lungeTarget) > 0.1f)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, lungeTarget, lungeSpeed * Time.deltaTime));
            yield return null;
        }

        Debug.Log("Player hit by SlingShot!");
        yield return new WaitForSeconds(0.5f); //Short delay after hitting player

        isRetreating = true;
        while (Vector2.Distance(rb.position, originalPos) > 0.1f)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, originalPos, lungeSpeed * Time.deltaTime));
            yield return null;
        }

        yield return new WaitForSeconds(2f); // Wait 2 seconds before next lunge

        isLunging = false;
        isRetreating = false;
    }
}

