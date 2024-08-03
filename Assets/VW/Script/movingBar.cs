using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingBar : MonoBehaviour
{
    public float speed = 2.0f;

    private RectTransform parentRectTransform;
    private float leftLimit;
    private float rightLimit;
    private bool movingRight = true;

    void Start()
    {
        parentRectTransform = transform.parent.GetComponent<RectTransform>();
        float parentWidth = parentRectTransform.rect.width;

        leftLimit = -parentWidth / 2;
        rightLimit = parentWidth / 2;
    }

    void Update()
    {
        if (movingRight)
        {
            transform.localPosition += Vector3.right * speed * Time.deltaTime;
            if (transform.localPosition.x >= rightLimit)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.localPosition -= Vector3.right * speed * Time.deltaTime;
            if (transform.localPosition.x <= leftLimit)
            {
                movingRight = true;
            }
        }
    }
}
