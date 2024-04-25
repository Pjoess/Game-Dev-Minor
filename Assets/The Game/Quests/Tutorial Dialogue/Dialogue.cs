using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{

    public static event System.Action<string[]> ChangeLines;


    [SerializeField]
    public string[] lines;
    public bool isTriggered{get;set;}

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player" && !isTriggered){
            ChangeLines?.Invoke(lines);
            Debug.Log("changing lines");
            isTriggered = true;
        }
    }
}
