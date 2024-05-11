using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningBox : MonoBehaviour
{

    public int damage;

    void Start(){
        damage = 1;
    }


    void Update()
    {
        transform.Rotate(0, 50 * Time.deltaTime, 0, Space.Self);
    }


    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            IDamageble damagable = other.GetComponent<IDamageble>();
            damagable.Hit(damage);
        }
    }
}
