using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetTriggers : MonoBehaviour
{
    [SerializeField]public static GameObject[] children;

    public GameObject[] childrens;
    void Start(){
        children = childrens;
    }

    public static void ResetAllTriggers(){
        foreach(GameObject go in children){
            go.SetActive(true);
        }
    }
}
