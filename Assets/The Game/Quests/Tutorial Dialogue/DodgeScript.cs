using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeScript : MonoBehaviour
{

    public Transform resetPosition;
    public bool isHit;

    private void Awake() {
        isHit = false;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            other.GetComponent<IDamageble>().Hit(1);
            if(other.GetComponent<Player_Manager>().playerState!= other.GetComponent<Player_Manager>().dashState){
                ResetPosition(other.gameObject);
            }
            
        }
    }

    void ResetPosition(GameObject player){
        player.transform.position = resetPosition.position;
    }


}
