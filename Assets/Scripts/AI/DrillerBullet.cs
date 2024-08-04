using UnityEngine;
using static Enemy;

public class DrillerBullet : MonoBehaviour
{
    public float speed;
    private Vector3 direction;
    private int damage = 1;
    private PlayerLife playerLife;

    void Start()
    {
        Destroy(gameObject, 3f);
        playerLife = GameObject.Find("Player").GetComponent<PlayerLife>();
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
        if (collision.gameObject.CompareTag("Player"))
        {
            playerLife.TakeDamage(damage);
        }
    }
}
