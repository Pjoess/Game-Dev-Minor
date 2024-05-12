using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [HideInInspector] private SphereCollider sphereCollider;

    private Vector3 direction;
    public float moveSpeed;
    public float lifetime;
    private Player_Manager player;

    void Start()
    {
        player = FindObjectOfType<Player_Manager>();
        sphereCollider = GetComponent<SphereCollider>();
        direction = Blackboard.instance.GetPlayerPosition() - transform.position;
        direction += Vector3.up * 1f;
        direction.Normalize();

        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }

    private void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * direction, Space.World);
        lifetime -= Time.deltaTime;
        if( lifetime <= 0 ) Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.Hit(5);
            Vector3 point = other.ClosestPoint(transform.position);
            player.ApplyKnockback(point, 50);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Obstacle")) Destroy(gameObject);
    }
}
