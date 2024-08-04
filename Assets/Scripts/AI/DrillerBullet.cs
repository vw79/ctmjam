using UnityEngine;

public class DrillerBullet : MonoBehaviour
{
    public float speed;
    private Vector3 direction;

    void Start()
    {
        Destroy(gameObject, 3f); // Destroy the bullet after 5 seconds to prevent it from going on forever
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
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // Adjust -90 if necessary based on your sprite's orientation
    }
}
