using UnityEngine;

public class NewBuddyMortar : MonoBehaviour
{
    public float range = 3.5f;
    public float destroyTime = 1f;
    public float fallSpeed = 8f;

    private bool explode = false;

    public GameObject fallingSphere;
    public ParticleSystem Explosion;

    // Update is called once per frame
    void Update()
    {
        fallingSphere.transform.Translate(fallSpeed * Time.deltaTime * Vector3.down, Space.World);

        if(Vector3.Distance(fallingSphere.transform.position, transform.position) < 0.2f && !explode)
        {
            explode = true;
            Explosion.Play();
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, LayerMask.GetMask("Enemy"));
            foreach(Collider collider in hitColliders)
            {
                IDamageble damageble = collider.gameObject.GetComponentInParent<IDamageble>();
                if (damageble != null) damageble.Hit(15);
            }
            fallingSphere.gameObject.SetActive(false);
            Destroy(gameObject, destroyTime);
        }
    }
}
