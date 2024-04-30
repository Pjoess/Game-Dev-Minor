using UnityEngine;

namespace buddy
{
    public class Blackboard : MonoBehaviour
    {
        public static Blackboard instance;

        private Player_Manager player;

        public Animator animator;
        [HideInInspector] public int animIDWalk;
        [HideInInspector] public int animIDShooting;
        [HideInInspector] public int animIDShootingMortar;

        public void AssignAnimIDs()
        {
            animIDWalk = Animator.StringToHash("isWalking");
            animIDShooting = Animator.StringToHash("isShooting");
            animIDShootingMortar = Animator.StringToHash("isShootingMortar");
        }

        private void Awake()
        {
            AssignAnimIDs();
            instance = this;
        }

        private void Start()
        {
            animator = GetComponent<Animator>();
            player = FindObjectOfType<Player_Manager>();
        }

        public Vector3 GetPlayerPosition()
        {
            if (player != null)
            {
                return player.transform.position;
            }
            else
            {
                Debug.LogError("Player_Manager not found!");
                return Vector3.zero;
            }
        }
    }
}
