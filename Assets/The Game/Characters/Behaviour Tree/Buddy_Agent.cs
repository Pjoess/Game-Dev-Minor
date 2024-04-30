using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace buddy
{
    public class Agent_Manager : MonoBehaviour
    {
        private IBaseNode agentBT = null;
        public NavMeshAgent agent;

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
