using System.Collections.Generic;
using UnityEngine;

public class ShowSlimes : MonoBehaviour
{
    public List<GameObject> slimes;

    public void ToggleSlimes(){
        foreach(var slime in slimes){
            slime.SetActive(true);
        }
    }
}
