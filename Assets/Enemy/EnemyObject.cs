using UnityEngine;

public class Enemy : EnemyCube
{
    private Transform player; // Reference to the player object

    // Override or extend behavior as needed
    protected override void Start()
    {
        base.Start(); // Call base class Start method if needed

        // Assuming you have a Player tag for the player object
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found. Make sure you have a GameObject with the 'Player' tag.");
        }
    }

    protected override void Update()
    {
        base.Update(); // Call base class Update method if needed

        // Face the player
        if (player != null)
        {
            FacePlayer();
        }
    }

    private void FacePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0f, directionToPlayer.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    protected override bool IsWeaponCollisionValid(Collider other)
    {
        return base.IsWeaponCollisionValid(other); // Call base class method if needed
    }
}
