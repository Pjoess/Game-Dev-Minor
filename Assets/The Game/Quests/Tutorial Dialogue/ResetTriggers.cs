using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetTriggers : MonoBehaviour
{
    public static GameObject[] children;

    void Awake(){

    }

    public static void ResetAllTriggers(){
        foreach(GameObject go in children){
            go.SetActive(true);
        }
    }
}
