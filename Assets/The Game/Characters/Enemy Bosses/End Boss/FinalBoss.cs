using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{

    public List<AttackPatternSO> allAttackPaterns;

    [HideInInspector] public bool isAttacking = false;
    public float attackPatternIntervalTime = 3f;

    [HideInInspector] public bool canRotate = true;
    public float rotationSpeed = 8f;

    public GameObject bulletPrefab;
    public GameObject mortarPrefab;

    private IBaseNode BTRootNode;

    private void CreateBT()
    {
        var facePlayer = new FacePlayerNode(this);
        var attackPlayer = new AttackPatternNode(this);

        List<IBaseNode> list = new List<IBaseNode>();
        {
            list.Add(facePlayer);
            list.Add(attackPlayer);
        }

        BTRootNode = new SequenceNode(list);
    }

    void Start()
    {
        CreateBT();
    }

    void Update()
    {
        BTRootNode?.Update();
    }

    public List<AttackAction> GetRandomPattern()
    {
        return allAttackPaterns[Random.Range(0, allAttackPaterns.Count)].GetActions();
    }

}
