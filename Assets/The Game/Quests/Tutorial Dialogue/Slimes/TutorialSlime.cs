using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TutorialSlime : MonoBehaviour , IDamageble
{
        public float dashSpeedMultiplier = 1.5f;

        public LayerMask playerLayer;

        public int damage;
        
        [SerializeField]public EnemyHealthBar enemyHealthBar;

        public int MaxHealthPoints { get { return maxHealthPoints; } }
        [SerializeField] private int maxHealthPoints = 15;
        public int HealthPoints { get { return healthPoints; } set { healthPoints = value; } }
        private int healthPoints;
        
    #region Unity Start Functions
    public void Awake()
        {
            //AssignAnimIDs();
            enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
            HealthPoints = MaxHealthPoints;
        }
    #endregion

    #region CheckStates

    #endregion

    #region State Logic Functions

        #region Receive Damage
            public void Hit(int damage)
            {
                HealthPoints -= damage;
                enemyHealthBar.UpdateHealthBar(HealthPoints, MaxHealthPoints);

                if(HealthPoints <= 0)
                {
                    Destroy(this.gameObject);
                }
            }
        #endregion
    #endregion


    #region Triggers/Collisions

    #endregion

    #region Animator
    // public void EndAttack()
    // {
    //     animator.SetBool(animIDAnticipate, false);
    //     animator.SetBool(animIDAttack, false);
    // }
    #endregion
}
