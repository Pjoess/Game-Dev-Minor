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
            gameObject.SetActive(false);
        }
    }

    // public void Start(){
    //     StartCoroutine(CheckTrigger());
    // }

    // public void Update(){
    //     if(DialogueManager.instance != null){
    //         if(!DialogueManager.instance.isActive){
    //             collider.isTrigger = true;
    //         }
    //     }
    // }

    // public IEnumerator CheckTrigger(){
    //     yield return new WaitForSeconds(1f);

    //     if(!DialogueManager.instance.isActive){
    //         collider.isTrigger = true;
    //     }
    // }

    // public static void SetTrigger(bool trigger){
    //     collider.isTrigger = trigger;
    // }
}
