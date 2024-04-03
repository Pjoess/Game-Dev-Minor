using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryDropScipt : MonoBehaviour
{
    public GameObject prefab;

    public void DropItem(Vector3 pos)
    {
        Instantiate(prefab, pos, Quaternion.identity);
    }
}
