using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class terrainTile : MonoBehaviour
{
    [SerializeField] Vector2Int tilePosition;

    void Start()
    {
        GetComponentInParent<infiniteWorld>().Add(gameObject, tilePosition);
    }

}
