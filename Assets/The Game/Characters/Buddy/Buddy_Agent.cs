using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections.Generic;
using UnityEditor.UIElements;

namespace buddy
{
    public class Agent_Manager : MonoBehaviour
    {
        private IBaseNode buddyBT = null;
        private Rigidbody rigidBody; // this is important otherwise the Bullets don't work
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

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            rigidBody = GetComponent<Rigidbody>();
        }

        void Start()
        {
            BuddyBehaviourTree();
        }

        void Update()
        {
            buddyBT?.Update();
        }

        private void BuddyBehaviourTree()
        {
            List<IBaseNode> buddyMovement = new()
            {
                new FollowNode(agent),
                new IdleNode(agent),
            };

            List<IBaseNode> enemyInLineOfSight = new()
            {
                new ShootBulletNode(agent, shootingRange, attackLayer, bulletShootHeight, bulletSpeed, bulletLifetime, bulletPrefab),
                new ShootMortarNode(agent, shootingRange, attackLayer, mortarSpawnHeight, mortarPrefab, buddyCooldownText, mortarCooldownTime)
            };

            List<IBaseNode> selectNode = new()
            {
                new SequenceNode(buddyMovement),
                new SequenceNode(enemyInLineOfSight),
            };

            buddyBT = new SelectorNode(selectNode);
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
