using UnityEngine;

public class Mortar : MonoBehaviour
{
    [HideInInspector] private SphereCollider sphereCollider;
    private float speed = 5f; // Speed of downward movement

    void Start()
    {
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
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<IDamageble>().Hit(5);
            Destroy(this.gameObject);
        }
    }

    public void DisableBullet() => sphereCollider.enabled = false;
    public void EnableBullit() => sphereCollider.enabled = true;
}
