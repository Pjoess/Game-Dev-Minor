using UnityEngine;

public class EnemyManagement : MonoBehaviour
{
    public float healthPoints = 2f;
    public float timerDuration = 1f;
    public float pushForce = 3f;
    public float friction = 2f; // Adjust the friction value as needed
    private bool isKnockedBack = false;
    private bool isCollisionCooldown = false; // Flag to track if the enemy is in collision cooldown
    public float collisionCooldown = 0.1f; // Time during which the enemy is immune to collision after being hit
    private float maxHeight = 15f; // Set your desired maximum height

    // On hit, change color and apply force.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon") && !isCollisionCooldown)
        {
            ApplyForce();
            switch (healthPoints)
            {
                case 2:
                    healthPoints--;
                    this.gameObject.GetComponent<MeshRenderer>().material.color = new Color32(170, 0, 0, 200);
                    transform.localScale *= 0.8f; // Make object smaller
                    print("Hit " + healthPoints);
                    break;
                case 1:
                    healthPoints--;
                    this.gameObject.GetComponent<MeshRenderer>().material.color = new Color32(70, 0, 0, 200);
                    transform.localScale *= 0.6f;
                    print("Hit " + healthPoints);
                    break;
                case 0:
                    healthPoints = 2;
                    this.gameObject.GetComponent<MeshRenderer>().material.color = new Color32(10, 0, 0, 200);
                    transform.localScale *= 0.2f;
                    print("Hit " + healthPoints);
                    Invoke(nameof(DestroyObject), timerDuration); // destroy object after a certain time
                    break;
            }

            // Start the collision cooldown
            StartCollisionCooldown();
        }
    }

    private void StartCollisionCooldown()
    {
        isCollisionCooldown = true;
        Invoke(nameof(EndCollisionCooldown), collisionCooldown);
    }

    private void EndCollisionCooldown()
    {
        isCollisionCooldown = false;
    }

    private void ApplyForce()
    {
        // Apply a force to push the enemy back
        Vector3 pushDirection = transform.forward; // Adjust the direction as needed
        GetComponent<Rigidbody>().AddForce(pushDirection * pushForce, ForceMode.VelocityChange);
        isKnockedBack = true;
    }

    private void DestroyObject()
    {
        // Destroy the GameObject after the specified duration
        Destroy(gameObject);
    }

    private void Update()
    {
        KnockBack();        
    }

    private void KnockBack()
    {
        if (isKnockedBack)
        {
            // Apply friction to decelerate the enemy
            GetComponent<Rigidbody>().velocity -= friction * Time.deltaTime * GetComponent<Rigidbody>().velocity;

            // Check if the velocity is low enough to stop
            if (GetComponent<Rigidbody>().velocity.magnitude < 0.1f)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                isKnockedBack = false;
            }
        }
        MaxHeightAfterHit();
    }

    private void MaxHeightAfterHit()
    {
        // Restrict the enemy's Y position
        if (transform.position.y > maxHeight)
        {
            Vector3 newPos = transform.position;
            newPos.y = maxHeight;
            transform.position = newPos;
        }
    }

    void OnTriggerStay(Collider other)
    {
        //Debug.Log("Staying in the collision...");
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Exiting collision...");
    }
}