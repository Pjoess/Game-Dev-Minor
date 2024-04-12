using System.Collections;
using UnityEngine;

public partial class Enemy
{
    #region Basic Variables
        public int MaxHealthPoints { get { return maxHealthPoints; } }
        public int HealthPoints { get { return healthPoints; } set { healthPoints = value; } }
        [SerializeField] private int maxHealthPoints = 3;
        private int healthPoints;
        public float respawnTime = 2f;
        public float pushBackForce = 4f;
        public float pushUpForce = 4f;
        public float pushbackGroundFriction = 2f;
        public bool isKnockedBack = false;
        public bool isCollisionCooldown = false;
        public float collisionCooldown = 1f;
        public float rotationSpeed = 750f;
    #endregion

    #region CheckTriggers
        public bool IsAggroed { get; set; }
        public bool IsWithinStrikingDistance { get; set; }
        public void SetAggroStatus(bool isAggroed) { IsAggroed = isAggroed; }
        public void SetStrikingDistanceBool(bool isWithinStrikingDistance) { IsWithinStrikingDistance = isWithinStrikingDistance; }
    #endregion

    #region Enemy Save Original Size and Position for the Respawn
        private Vector3 initialPosition, originalScale;
        private Quaternion originalRotation;
        private Color originalColor;
    #endregion

    #region Audio
        public AudioSource slimeJumpSound;
    #endregion

    #region References
        private Transform player; // Reference to the player object
        private HealthDropScript healthDropScript;
    #endregion
}
