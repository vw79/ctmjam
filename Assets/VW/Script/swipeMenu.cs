using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class swipeMenu : MonoBehaviour
{
    public GameObject scrollbar;
    private float scroll_pos = 0;
    float[] pos;
    private string selectedCharacterName;

    void Start()
    {
        pos = new float[transform.childCount];
    }

    void Update()
    {
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
            }
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                    selectedCharacterName = transform.GetChild(i).name; 
                }
            }
        }

        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                for (int a = 0; a < pos.Length; a++)
                {
                    if (a != i)
                    {
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.6f, 0.6f), 0.1f);
                    }
                }
            }
        }

        //Debug.Log(selectedCharacterName);
    }

    public void MoveLeft()
    {
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                if (i > 0)
                {
                    StartCoroutine(SmoothScroll(scroll_pos, pos[i - 1], 0.2f));
                    break;
                }
            }
        }
    }

    public void MoveRight()
    {
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                if (i < pos.Length - 1)
                {
                    StartCoroutine(SmoothScroll(scroll_pos, pos[i + 1], 0.2f));
                    break;
                }
            }
        }
    }

    private IEnumerator SmoothScroll(float startValue, float endValue, float duration)
    {
        float elapsed = 0f;
        Scrollbar scrollbarComponent = scrollbar.GetComponent<Scrollbar>();

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            scrollbarComponent.value = Mathf.Lerp(startValue, endValue, elapsed / duration);
            yield return null;
        }

        scrollbarComponent.value = endValue;
        scroll_pos = endValue;
    }


    public string GetSelectedCharacterName()
    {
        return selectedCharacterName;
    }
}