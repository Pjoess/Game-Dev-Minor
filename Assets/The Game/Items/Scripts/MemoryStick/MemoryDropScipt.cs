using UnityEngine;

public class MemoryDropScipt : MonoBehaviour
{
    public GameObject prefab;

    public void DropItem(Vector3 pos)
    {
        pos.y += 1; // Quick fix, slime has design error so force to spawn on a different position (can be deleted)
        Instantiate(prefab, pos, Quaternion.identity);
    }
}
