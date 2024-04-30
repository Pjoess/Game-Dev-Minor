using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections.Generic;

namespace buddy
{
    public class Agent_Manager : MonoBehaviour
    {
        private IBaseNode agentBT = null;
        private Rigidbody rigidBody;
        public NavMeshAgent agent;
        public GameObject bulletPrefab;
        public GameObject mortarPrefab;
        public LayerMask attackLayer;
        public AudioSource shootSound;
        

        [Header("Attack")]
        public int shotsFired = 0;
        public float shootingRange = 10f;
        public float bulletSpeed = 8f;
        public float bulletLifetime = 5f;
        public float bulletShootHeight = 1f;
        public float mortarSpeed = 5f;
        public float distanceToMove = 5f;
        public float mortarSpawnHeight = 8f;

        [Header("Cooldown")]
        [SerializeField] private TMP_Text buddyCooldownText;
        [SerializeField] private float mortarCooldownTime = 3f;
        private float nextMortarTime = 0f;

        private Animator animator;
        [HideInInspector] public int animIDWalk;
        [HideInInspector] public int animIDShooting;
        [HideInInspector] public int animIDShootingMortar;

        private void AssignAnimIDs()
        {
            animIDWalk = Animator.StringToHash("isWalking");
            animIDShooting = Animator.StringToHash("isShooting");
            animIDShootingMortar = Animator.StringToHash("isShootingMortar");
        }

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        void Start()
        {
            CreateBehaviourTree();
        }

        void Update()
        {
            agentBT?.Update();
        }

        private void CreateBehaviourTree()
        {
            List<IBaseNode> movement = new()
            {
                new IdleNode(agent),
                new FollowNode(agent),
            };

            List<IBaseNode> enemyLineOfSight = new()
            {
                
            };

            List<IBaseNode> selectNode = new()
            {
                new SequenceNode(movement),
                new SequenceNode(enemyLineOfSight),
            };

            agentBT = new SelectorNode(selectNode);
        }

        // Draw Gizmos to visualize the detection cones
        void OnDrawGizmosSelected()
        {
            if (agent != null)
            {
            
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {

            }
        }
    }
}
