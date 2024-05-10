using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalBoss : MonoBehaviour, IDamageble
{


    private int healthPoints;
    [SerializeField] private int maxHealthPoints = 100;
    public int MaxHealthPoints { get { return maxHealthPoints; } }
    [HideInInspector] public int HealthPoints { get { return healthPoints; } set { healthPoints = value; } }

    [SerializeField] private Canvas bossUI;
    [SerializeField] private Slider bossHealthBar;

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
        healthPoints = maxHealthPoints;
        bossHealthBar.maxValue = maxHealthPoints;
        bossHealthBar.value = healthPoints;
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

    public void Hit(int damage)
    {
        healthPoints -= damage;
        bossHealthBar.value = healthPoints;
        CheckDead();
    }

    private void CheckDead()
    {
        if(healthPoints <= 0)
        {
            Destroy(gameObject);
            bossUI.gameObject.SetActive(false);
        }
    }
}
