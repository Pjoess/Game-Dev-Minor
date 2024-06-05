using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

namespace SlimeMiniBoss
{
    public class SlimeMiniBoss_Agent : MonoBehaviour, IDamageble
    {
        private IBaseNode slimeBT = null;
        public LayerMask attackLayer; // Player
        private NavMeshAgent miniBossAgent;
        private Rigidbody rigidBody;
        private ParticleSystem shockwaveParticleSystem;
        private List<Color> originalHelmetColors = new List<Color>();
        private Renderer[] helmetRenderers;

        private GameObject bone;

        private DecalProjector decalProjector;
        [SerializeField] Material neutralFace, hitFace, deadFace;
        [SerializeField] private ParticleSystem deathParticle;
        [SerializeField] private float deathTimer = 2;
        private bool isAlive = true;

        [Header("Patrol Center Point")]
        public GameObject patrolCenterPoint;

        [Header("Chase")]
        private float chaseRange = 20f;

        [Header("Attack")]
        private float attackRange = 19f;
        private float offsetDistance = 1f;
        public static bool hasAttacked = false;
        
        // --- IDamagable --- //
        [Header("Stats")]
        private EnemyHealthBar enemyHealthBar;
        public int healthPoints;
        public int maxHealthPoints = 50;
        public int MaxHealthPoints { get { return maxHealthPoints; } }
        public int HealthPoints { get { return healthPoints; } set { healthPoints = value; } }
    
        [Header("Cone Settings")]
        private float coneWidth = 50f;
        private float coneLength = 10f;
        private float thickness = 2f;

        [Header("Patrol Settings")]
        private float patrolRadius = 20f;
        private float stopDistance = 4f;

        private Animator animator;
        private int animIDAnticipate;
        private int animIDAttack;

        private void AssignAnimIDs()
        {
            animIDAnticipate = Animator.StringToHash("isAnticipating");
            animIDAttack = Animator.StringToHash("isAttacking");
        }

        private void Awake()
        {
            AssignAnimIDs();
            HealthPoints = MaxHealthPoints;
            animator = GetComponent<Animator>();
            miniBossAgent = GetComponent<NavMeshAgent>();
            enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
            rigidBody = GetComponent<Rigidbody>();
            shockwaveParticleSystem = GetComponentInChildren<ParticleSystem>();
            decalProjector = GetComponentInChildren<DecalProjector>();
            bone = transform.Find("Armature").Find("Bone").gameObject;
        }

        void Start()
        {
            MiniBossSlimeBehaviourTree();
            SaveOriginalHelmetColors();
        }

        void Update()
        {
            if(isAlive)
            {
                slimeBT?.Update(); // Update the boss behavior tree
            }
        }

        #region Behaviour Tree
        private void MiniBossSlimeBehaviourTree()
        {
            //PatrolChaseAttack();
            OnlyAttackAndRotate();
        }

        private void OnlyAttackAndRotate()
        {
            List<IBaseNode> IsPlayerInLineOfSight = new()
            {
                new AttackPlayerOnPositionNode(miniBossAgent, attackRange, offsetDistance, attackLayer, coneWidth, coneLength,animator,animIDAnticipate,animIDAttack),
            };

            slimeBT = new SelectorNode(IsPlayerInLineOfSight);
        }

        // Removed for now, changed to mini slime boss stays on its spot and just looks at the player.
        private void PatrolChaseAttack()
        {
            List<IBaseNode> IsPlayerInLineOfSight = new()
            {
                new ChasePlayerNode(miniBossAgent, chaseRange, stopDistance),
                new AttackPlayerNode(miniBossAgent, attackRange, offsetDistance, attackLayer, coneWidth, coneLength,animator,animIDAnticipate,animIDAttack),
            };

            List<IBaseNode> IsPlayerNotInLineOfSight = new()
            {
                new PatrolNode(miniBossAgent, patrolCenterPoint, patrolRadius, stopDistance, chaseRange),
            };

            List<IBaseNode> Root = new()
            {
                new SequenceNode(IsPlayerInLineOfSight),
                new SequenceNode(IsPlayerNotInLineOfSight),
            };

            slimeBT = new SelectorNode(Root);
        }
        #endregion

        #region Save and Change Helmet Colors
        private void SaveOriginalHelmetColors()
        {
            Transform helmetTransform = transform.Find("helmet");
            if (helmetTransform != null)
            {
                helmetRenderers = helmetTransform.GetComponentsInChildren<Renderer>();
                foreach (var renderer in helmetRenderers)
                {
                    originalHelmetColors.Add(renderer.material.color);
                }
            }
        }

        private IEnumerator ChangeColorOnHit()
        {
            if (helmetRenderers != null)
            {
                Color lightRed = new(255 / 255f, 51 / 255f, 51 / 255f, 1f);
                SetHitFace();
                foreach (var renderer in helmetRenderers)
                {
                    renderer.material.color = lightRed;
                }
                yield return new WaitForSeconds(0.2f);
                SetNeutralFace();
                for (int i = 0; i < helmetRenderers.Length; i++)
                {
                    helmetRenderers[i].material.color = originalHelmetColors[i];
                }
            }
        }
        #endregion

        #region Cone Raycast
        // Draw Gizmos for cone shape and attack range
        private void OnDrawGizmos()
        {
            // Draw cone shape in Gizmos
            DrawCone(transform.position, transform.forward, coneWidth, coneLength, thickness);

            // *** Commented so because the mini boss changed to not moving, but only rotating *** //
            // Draw chase range sphere
            // Gizmos.color = Color.blue;
            // Gizmos.DrawWireSphere(transform.position, chaseRange);

            // Draw stop distance
            // Gizmos.color = Color.yellow;
            // Gizmos.DrawWireSphere(transform.position, stopDistance);

            // Draw attack range sphere
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            // Draw patrol radius
            if (patrolCenterPoint != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(patrolCenterPoint.transform.position, patrolRadius);
            }
        }

        // Draw cone shape in Gizmos
        private void DrawCone(Vector3 origin, Vector3 direction, float coneWidth, float coneLength, float thickness)
        {
            // Store the current Gizmos color
            Color previousColor = Gizmos.color;

            // Set the desired color
            Gizmos.color = Color.red;

            // Calculate half width
            float halfWidth = coneWidth / 2f;

            // Calculate offset for thickness
            Vector3 offset = Vector3.up * thickness;

            // Draw cone base with thickness
            Gizmos.DrawLine(origin - offset, origin + Quaternion.Euler(0, -halfWidth, 0) * direction * coneLength - offset);
            Gizmos.DrawLine(origin - offset, origin + Quaternion.Euler(0, halfWidth, 0) * direction * coneLength - offset);
            Gizmos.DrawLine(origin + offset, origin + Quaternion.Euler(0, -halfWidth, 0) * direction * coneLength + offset);
            Gizmos.DrawLine(origin + offset, origin + Quaternion.Euler(0, halfWidth, 0) * direction * coneLength + offset);

            // Draw cone sides with thickness
            Gizmos.DrawLine(origin - offset, origin + Quaternion.Euler(0, -halfWidth, 0) * direction * coneLength / Mathf.Cos(Mathf.Deg2Rad * halfWidth) - offset);
            Gizmos.DrawLine(origin - offset, origin + Quaternion.Euler(0, halfWidth, 0) * direction * coneLength / Mathf.Cos(Mathf.Deg2Rad * halfWidth) - offset);
            Gizmos.DrawLine(origin + offset, origin + Quaternion.Euler(0, -halfWidth, 0) * direction * coneLength / Mathf.Cos(Mathf.Deg2Rad * halfWidth) + offset);
            Gizmos.DrawLine(origin + offset, origin + Quaternion.Euler(0, halfWidth, 0) * direction * coneLength / Mathf.Cos(Mathf.Deg2Rad * halfWidth) + offset);

            // Draw lines to connect cone sides to cone base with thickness
            Gizmos.DrawLine(origin - offset, origin - offset);
            Gizmos.DrawLine(origin + offset, origin + offset);
            Gizmos.DrawLine(origin + Quaternion.Euler(0, -halfWidth, 0) * direction * coneLength - offset, origin + Quaternion.Euler(0, -halfWidth, 0) * direction * coneLength + offset);
            Gizmos.DrawLine(origin + Quaternion.Euler(0, halfWidth, 0) * direction * coneLength - offset, origin + Quaternion.Euler(0, halfWidth, 0) * direction * coneLength + offset);

            // Restore the previous Gizmos color
            Gizmos.color = previousColor;
        }
        #endregion

        #region IDamagable
        public void Hit(int damage)
        {
            if(isAlive)
            {
                HealthPoints -= damage;
                enemyHealthBar.UpdateHealthBar(HealthPoints, MaxHealthPoints);
                CheckDeath();
                if(isAlive) StartCoroutine(ChangeColorOnHit());
            }
        }

        private void CheckDeath()
        {
            if (HealthPoints <= 0)
            {
                
                //save the bone postion for later
                var pos = bone.transform.position;

                //reset the controller to force the slime into a neutral pose
                var controller = animator.runtimeAnimatorController;
                animator.runtimeAnimatorController = null;
                animator.runtimeAnimatorController = controller;

                //Reset the animator parameters just to be sure
                //nothing strange happens
                animator.SetBool(animIDAnticipate, false);
                animator.SetBool(animIDAttack, false);

                //disable the animator to stop animation
                animator.enabled = false;

                //Set the postition to the saved bone position, so that the slime
                //will remain at the position it was
                transform.position = pos;

                //Disable navagent and kinematic for the knockback
                miniBossAgent.enabled = false;
                rigidBody.isKinematic = false;

                isAlive = false;
                StartCoroutine(Dead());
            }
        }

        private IEnumerator Dead()
        {
            SetDeadFace();
            ApplyKnockback(Blackboard.instance.GetPlayerPosition(), 300);
            yield return new WaitForSeconds(deathTimer);
            ParticleSystem par = Instantiate(deathParticle, transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
            GetComponent<MemoryDropScipt>().DropItem(transform.position);
            Destroy(gameObject);
        }

        public void ApplyKnockback(Vector3 direction, int force)
        {
            Vector3 pushDirection = transform.position - direction;
            pushDirection.y = 0;
            pushDirection.Normalize();
            rigidBody.AddForce(pushDirection * force, ForceMode.Impulse);
        }
        #endregion

        #region Animator
        public void DoShockwaveAttack()
        {
            hasAttacked = true;
            shockwaveParticleSystem.Play();
        }

        public void EndAnticipate()
        {
            animator.SetBool(animIDAttack, true);
        }

        public void EndAttack()
        {
            hasAttacked = false;
            animator.SetBool(animIDAnticipate, false);
            animator.SetBool(animIDAttack, false);
        }
        #endregion

        private void SetNeutralFace()
        {
            decalProjector.material = neutralFace;
        }

        private void SetHitFace()
        {
            decalProjector.material = hitFace;
        }

        private void SetDeadFace()
        {
            decalProjector.material = deadFace;
        }
    }
}
