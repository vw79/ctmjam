using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float stopDistance = 2f;
    private Rigidbody2D rb;
    private Vector2 movement;

     void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();    
    }

     void Update()
    {
        Vector3 direction = player.position - transform.position;
       
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

    private void FixedUpdate()
    {
        moveEnemy(movement);
    }

    void moveEnemy(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }
}
