using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoins : MonoBehaviour
{
    private static int counter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            counter++;
            Destroy(gameObject);
            SoundManager.instance.Play("CollectCoins");
            Debug.Log("Coins Collected: " +  counter);
        }
    }
}
