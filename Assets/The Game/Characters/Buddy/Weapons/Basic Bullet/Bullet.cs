using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] private SphereCollider sphereCollider;

    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponentInParent<IDamageble>().Hit(1);
            Destroy(this.gameObject);
        }
    }

    public void DisableBullet() => sphereCollider.enabled = false;
    public void EnableBullit() => sphereCollider.enabled = true;
}
