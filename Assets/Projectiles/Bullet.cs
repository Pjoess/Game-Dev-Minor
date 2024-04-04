using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] private SphereCollider sphereCollider;

    // Start is called before the first frame update
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        //EnableBullit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // void OnTriggerEnter(Collider other)
    // {
    //     if(other.CompareTag("Enemy")){
    //         DisableBullet();
    //     }
    // }

    // void OnTriggerExit(Collider other)
    // {
    //     if(other.CompareTag("Enemy")){
    //         EnableBullit();
    //     }
    // }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<IDamageble>().Hit(0);
            Destroy(this.gameObject);
        }
    }

    public void DisableBullet() => sphereCollider.enabled = false;
    public void EnableBullit() => sphereCollider.enabled = true;
}
