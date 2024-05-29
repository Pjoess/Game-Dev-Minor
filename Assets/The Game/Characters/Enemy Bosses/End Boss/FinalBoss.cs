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

    [SerializeField] private ParticleSystem shockwave;
    public float meleeRange = 3f;
    public float stompCooldown = 2f;

    [HideInInspector] public bool isAttacking = false;
    public float attackPatternIntervalTime = 3f;

    [HideInInspector] public bool canRotate = true;
    public float rotationSpeed = 8f;

    private bool fightStarted = false;
    public bool phaseTwo = false;
    private float eyeColorIntensity = 0f;
    [SerializeField] Material eyesMaterial;
    private bool isDead = false;

    public GameObject bulletPrefab;
    public GameObject mortarPrefab;

    private IBaseNode BTRootNode;

    [HideInInspector] public Animator animator;
    [HideInInspector] public int animIDIsShooting;
    [HideInInspector] public int animIDIsMortarShooting;
    [HideInInspector] public int animIDIsStomping;
    [HideInInspector] public int animIDIsDead;

    private void AssignAnimIDs()
    {
        animIDIsShooting = Animator.StringToHash("isShooting");
        animIDIsMortarShooting = Animator.StringToHash("isMortarShooting");
        animIDIsStomping = Animator.StringToHash("isStomping");
        animIDIsDead = Animator.StringToHash("isDead");
    }


    private void CreateBT()
    {
        var facePlayer = new FacePlayerNode(this);
        var attackPlayer = new AttackPatternNode(this);
        var stomp = new StompNode(this);

        List<IBaseNode> attackLlist = new List<IBaseNode>();
        {
            attackLlist.Add(attackPlayer);
            attackLlist.Add(stomp);
        }

        SelectorNode attackNodes =  new SelectorNode(attackLlist);

        List<IBaseNode> list = new List<IBaseNode>();
        {
            list.Add(facePlayer);
            list.Add(attackNodes);
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
        eyeColorIntensity = 0f;
        eyesMaterial.SetColor("_BaseColor", Color.red);
        eyesMaterial.SetColor("_EmissionColor", Color.red * 0f);
        healthPoints = maxHealthPoints;
        bossHealthBar.maxValue = maxHealthPoints;
        bossHealthBar.value = healthPoints;
        CreateBT();
    }

    void Update()
    {
        if(fightStarted && !isDead)
        {
            BTRootNode?.Update();
        }

        if(healthPoints <= maxHealthPoints/2)
        {
            StartCoroutine(changeEyeColorPhaseTwo());
            phaseTwo = true;
        }
    }

    public List<AttackAction> GetRandomPattern()
    {
        return allAttackPaterns[Random.Range(0, allAttackPaterns.Count)].GetActions();
    }

    private IEnumerator changeEyeColorPhaseTwo()
    {
        while (eyeColorIntensity < 10f)
        {
            eyesMaterial.SetColor("_EmissionColor", Color.red * eyeColorIntensity);
            eyeColorIntensity += 0.01f;
            yield return null;
        }
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
            StartCoroutine(Death());
            
        }
    }


    private IEnumerator Death()
    {
        isDead = true;
        animator.SetBool(animIDIsDead, true);
        bossUI.gameObject.SetActive(false);
        eyesMaterial.SetColor("_EmissionColor", Color.black);
        eyesMaterial.SetColor("_BaseColor", Color.grey);
        yield return new WaitForSeconds(5f);
        QuestEvents.BuddyDead();
    }


    public void ActivateBoss()
    {
        fightStarted = true;
        bossUI.gameObject.SetActive(true);
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
        Instantiate(mortarPrefab, Blackboard.instance.GetPlayerPosition(), Quaternion.identity);
        animator.SetBool(animIDIsMortarShooting, false);
    }

    public void DoStomp()
    {
        shockwave.Play();
        animator.SetBool(animIDIsStomping, false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
