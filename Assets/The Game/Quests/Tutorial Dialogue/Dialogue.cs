using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public static event System.Action<string[]> ChangeLines;
    public bool isLastOne;

    public string[] lines;
    public bool IsTriggered{get;set;}

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player") && !IsTriggered){
            if(isLastOne){
                DialogueManager.instance.isLastOne = true;
                ChangeLines?.Invoke(lines);
                return;
            }
            ChangeLines?.Invoke(lines);
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
