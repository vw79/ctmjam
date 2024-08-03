using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelExplosion : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosion;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if (explosion != null) 
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }    
    }
}
