using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class hitBox : MonoBehaviour
{
    private GameObject coinVFX;

    private Vector3 initialScaleCoinVFX;
    private Vector3 initialScaleHitBox;
    private float scalingFactor = 0.01f;

    private bool enemyHit = false;

    void Start()
    {
        coinVFX = GameObject.Find("CoinsVFX");

        initialScaleCoinVFX = coinVFX.transform.localScale;
        initialScaleHitBox = transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemyHit = true;
        }
    }

    public bool CheckEnemyHit()
    {
        bool wasEnemyHit = enemyHit;
        enemyHit = false;
        if (wasEnemyHit)
        {
            ScaleObjects();
        }
        return wasEnemyHit;
    }

    private void ScaleObjects()
    {
        Vector3 newScaleCoinVFX = initialScaleCoinVFX * (1 + scalingFactor);
        Vector3 newScaleHitBox = initialScaleHitBox * (1 + scalingFactor);

        coinVFX.transform.localScale = newScaleCoinVFX;
        transform.localScale = newScaleHitBox;

        initialScaleCoinVFX = newScaleCoinVFX;
        initialScaleHitBox = newScaleHitBox;
    }
}