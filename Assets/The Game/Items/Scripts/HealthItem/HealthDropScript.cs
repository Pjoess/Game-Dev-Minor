using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthDropScript : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<HealthItem> healthItems = new List<HealthItem>();

    HealthItem GetDroppedItem()
    {
        int randomNumber = Random.Range(1, 101); //1-100
        List<HealthItem> items = new List<HealthItem>();
        foreach (HealthItem item in healthItems)
        {
            if(randomNumber <= item.dropChance)
            {
                items.Add(item);
            }
        }

        if(items.Count > 0)
        {
            HealthItem dropped = items[0];
            for (int i = 1; i < items.Count; i++)
            {
                if (items[i].dropChance < dropped.dropChance) dropped = items[i];
            }

            return dropped;
        }
        else return null;
    }

    public void InstantiateDroppedItem(Vector3 pos)
    {
        HealthItem dropped = GetDroppedItem();

        if(dropped != null)
        {
            GameObject obj = Instantiate(droppedItemPrefab, pos, Quaternion.identity);
            obj.GetComponentInChildren<HealthItemScript>().healthItem = dropped;
        }
    }
}
