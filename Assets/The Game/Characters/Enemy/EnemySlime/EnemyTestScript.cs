using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTestScript: MonoBehaviour {

    public float Range = 10f;
    public NavMeshAgent Agent;
    public GameObject Target;


    public void Awake(){
        Agent = GetComponent<NavMeshAgent>();
        Target = GameObject.FindWithTag("Player");
    }

    public void Update(){

        if(CheckRange()){
            Agent.SetDestination(Target.transform.position);
        }
    }



    public bool CheckRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, Range);
        foreach (var hitCollider in hitColliders)
        {
			if (hitCollider.gameObject.tag == "Player") {
				Debug.Log ("Attacking Now!");
                return true;
            } else{
                return false;
            }
        }

        return false;
    }
}