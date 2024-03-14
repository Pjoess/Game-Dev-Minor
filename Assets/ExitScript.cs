using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    GameObject exitObject;

    // Start is called before the first frame update
    void Start()
    {
        exitObject = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the tag "Sword"
        if (collision.gameObject.CompareTag("Sword"))
        {
            // Exit the application
            Application.Quit();
        }
    }
}
