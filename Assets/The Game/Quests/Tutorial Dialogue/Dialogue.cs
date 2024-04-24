using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : IDialogueTrigger
{
    [SerializeField]
    public string[] lines {get; set;}
    public bool isTriggered{get;set;}

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player" && !isTriggered){
            Debug.Log("changing lines");
            isTriggered = true;
        }
    }
}
