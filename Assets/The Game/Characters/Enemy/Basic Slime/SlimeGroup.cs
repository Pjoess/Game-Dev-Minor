using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeGroup : MonoBehaviour
{

    [SerializeField] private float range = 3f;

    [SerializeField] List<GameObject> slimes;

    void Start()
    {
        slimes = new List<GameObject>();
        GetSlimes();

    }

    // Update is called once per frame
    void Update()
    {
        if(AreAllSlimesDead())
        {
            TimeScript.instance.SlowMo();
            Destroy(gameObject);
        }
    }

    private void GetSlimes()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, range / 2, transform.forward, range, LayerMask.GetMask("Enemy"));
        if(hits.Length > 0 )
        {
            foreach(RaycastHit hit in hits )
            {
                slimes.Add(hit.collider.gameObject);
            }
        }
    }

    private bool AreAllSlimesDead()
    {
        foreach (GameObject s in slimes)
        {
            if (s != null) return false;
        }
        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
