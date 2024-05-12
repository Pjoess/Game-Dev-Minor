using UnityEngine;

public class BossMortar : MonoBehaviour
{
    [HideInInspector] private SphereCollider sphereCollider;
    public float speed = 7f; // Speed of downward movement
    private Player_Manager player;

    void Start()
    {
        player = FindObjectOfType<Player_Manager>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    void Update()
    {
        // Calculate the forward direction based on the current rotation
        Vector3 forwardDirection = transform.forward;

        // Move the mortar forward
        transform.Translate(forwardDirection * speed * Time.deltaTime, Space.World);

        // Rotate the mortar to point downwards
        transform.rotation = Quaternion.LookRotation(Vector3.down);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.Hit(15);
            player.ApplyKnockback(transform.position, 300);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.CompareTag("Obstacle")) Destroy(this.gameObject);
    }

    public void DisableBullet() => sphereCollider.enabled = false;
    public void EnableBullit() => sphereCollider.enabled = true;
}
