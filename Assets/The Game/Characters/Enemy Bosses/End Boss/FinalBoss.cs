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

    public float meleeRange = 3f;

    [HideInInspector] public bool isAttacking = false;
    public float attackPatternIntervalTime = 3f;

    [HideInInspector] public bool canRotate = true;
    public float rotationSpeed = 8f;

    public GameObject bulletPrefab;
    public GameObject mortarPrefab;

    private IBaseNode BTRootNode;

    [HideInInspector] public Animator animator;
    [HideInInspector] public int animIDIsShooting;
    [HideInInspector] public int animIDIsMortarShooting;

    private void AssignAnimIDs()
    {
        animIDIsShooting = Animator.StringToHash("isShooting");
        animIDIsMortarShooting = Animator.StringToHash("isMortarShooting");
    }


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

    private void Awake()
    {
        animator = GetComponent<Animator>();
        AssignAnimIDs();
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

    public bool IsAnimatorCurrentState(string name)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    public void ShootBullet()
    {
        Instantiate(bulletPrefab, transform.position + Vector3.up * 3f, Quaternion.identity);
        animator.SetBool(animIDIsShooting, false);
    }

    public void ShootMortar()
    {
        Instantiate(mortarPrefab, Blackboard.instance.GetPlayerPosition() + Vector3.up * 8f, Quaternion.identity);
        animator.SetBool(animIDIsMortarShooting, false);
    }
}
