using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 5.0f;
    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player GameObject is tagged as 'Player'.");
        }
    }

void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            Debug.Log(KeyCode.E);
            RotateCameraAroundPlayer(rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            Debug.Log(KeyCode.Q);
            RotateCameraAroundPlayer(-rotationSpeed);
        }
    }

    void RotateCameraAroundPlayer(float rotationAmount)
    {
        Debug.Log("RotateCameraAroundPlayer is being called.");
        transform.RotateAround(player.position, Vector3.up, rotationAmount * Time.deltaTime);
    }
}
