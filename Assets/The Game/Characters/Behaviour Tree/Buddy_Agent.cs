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
        public LayerMask attackLayer;
        public AudioSource shootSound;
        public GameObject bulletPrefab;
        public GameObject mortarPrefab; // Added mortar prefab

        [Header("Attack")]
        public int shotsFired = 0;
        public float shootingRange = 10f;
        public float bulletSpeed = 8f;
        public float bulletLifetime = 5f;
        public float bulletShootHeight = 1f;
        public float mortarSpeed = 5f;
        public float mortarSpawnHeight = 8f;
        public float distanceToMove = 5f;

        [Header("Cooldown")]
        public TMP_Text buddyCooldownText;
        [SerializeField] private float mortarCooldownTime = 3f;
        private float nextMortarTime = 0f;

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
            List<IBaseNode> movement = new List<IBaseNode>
            {
                new FollowNode(agent),
                new IdleNode(agent),
            };

            List<IBaseNode> enemyLineOfSight = new List<IBaseNode>
            {
                new ShootBulletNode(agent, shootingRange, attackLayer, bulletShootHeight, bulletSpeed, bulletLifetime, bulletPrefab),
                // Fixing the constructor of ShootMortarNode
                new ShootMortarNode(agent, shootingRange, attackLayer, mortarSpeed, mortarSpawnHeight, mortarPrefab, buddyCooldownText, mortarCooldownTime)
            };

            List<IBaseNode> selectNode = new List<IBaseNode>
            {
                new SequenceNode(movement),
                new SequenceNode(enemyLineOfSight),
            };

            agentBT = new SelectorNode(selectNode);
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {

            }
        }

        void OnDrawGizmosSelected()
        {
            // Draw a wire sphere to represent the shooting range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, shootingRange);

            // Draw a wire sphere to represent the distance to move
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, distanceToMove);
        }
    }
}
