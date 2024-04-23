using UnityEngine;

public class Mortar : MonoBehaviour
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
            other.gameObject.GetComponent<IDamageble>().Hit(20);
            Debug.Log("Mortar Hit");
            Destroy(this.gameObject);
        }
    }

    public void DisableBullet() => sphereCollider.enabled = false;
    public void EnableBullit() => sphereCollider.enabled = true;
}
