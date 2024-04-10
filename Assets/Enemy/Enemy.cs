using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

public partial class Enemy : MonoBehaviour, IDamageble
{
  
    public void Start()
    {
        healthDropScript = GetComponent<HealthDropScript>();
        healthPoints = maxHealthPoints;
        slimeJumpSound = GetComponent<AudioSource>();
        InitializeOriginalValues();
        StartCoroutine(JumpRoutine());
        CheckPlayerExist();

        //InvokeRepeating(nameof(FireBullet), 0f, fireInterval);
    }

    public void Update(){
        FacePlayer(); // Face the player
    }

    public void CheckPlayerExist()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) 
        {
            player = playerObject.transform;
        } 
        else 
        {
            Debug.LogError("Player not found. Make sure you have a GameObject with the 'Player' tag.");
        }
    }

    public void FacePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0f, directionToPlayer.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    //public void OnTriggerEnter(Collider other)
    //{
    //   if (IsWeaponCollisionValid(other))
    //   {
    //       ApplyDamageAndEffects();
    //       // CheckHealthAndUpdateAppearance();
    //   }
    //}

    //public bool IsWeaponCollisionValid(Collider other)
    //{
    //    return other.gameObject.CompareTag("Weapon") && !isCollisionCooldown;
    //}

    public void OnHealthUpdateChangeAppearance()
    {
        switch (healthPoints)
        {
            case 2: UpdateAppearance(new Color32(170, 0, 0, 200), originalScale * 0.8f); break;
            case 1: UpdateAppearance(new Color32(70, 0, 0, 200), originalScale * 0.6f); break;
            case 0: UpdateAppearance(new Color32(10, 0, 0, 200), originalScale * 0.2f);
                    StartCoroutine(RespawnEnemy()); // this is for testing purposes
                    break;
        }
    }


    public void StartCollisionCooldown()
    {
        isCollisionCooldown = true;
        Invoke(nameof(EndCollisionCooldown), collisionCooldown);
    }

    public void EndCollisionCooldown() => isCollisionCooldown = false;

    
    #region Enemy Jump
        public IEnumerator JumpRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1.5f);
                EnemyJump();
            }
        }

        public void EnemyJump()
        {
            if (healthPoints > 0)
            {
                GetComponent<Rigidbody>().AddForce(Vector3.up * pushUpForce, ForceMode.VelocityChange);
                if (slimeJumpSound != null)
                {
                    //slimeJumpSound.Play();
                }
            }
        }
    #endregion

    #region Start over and Respawn Enemy
        public void DestroyEnemy()
        {
            gameObject.SetActive(false);
            Debug.Log("Enemy Defeated!");
        }

        public IEnumerator RespawnEnemy()
        {
            healthDropScript.InstantiateDroppedItem(new Vector3(transform.position.x, initialPosition.y, transform.position.z));
            yield return new WaitForSeconds(respawnTime);
            UpdateAppearance(originalColor, originalScale);
            ResetEnemyStatePosition();
            gameObject.SetActive(true);
            Debug.Log("Enemy Respawned!");
        }

        public void ResetEnemyStatePosition()
        {
            healthPoints = 3;
            transform.position = initialPosition;
            transform.rotation = originalRotation;
        }

        public void UpdateAppearance(Color color, Vector3 scale)
        {
            GetComponent<MeshRenderer>().material.color = color;
            transform.localScale = scale;
        }

        public void InitializeOriginalValues()
        {
            initialPosition = transform.position;
            originalColor = GetComponent<MeshRenderer>().material.color;
            originalScale = transform.localScale;
            originalRotation = transform.rotation;
        }
    #endregion

    #region Combat
        public void ApplyDamageAndEffects() => healthPoints--;
        
        public void Hit(int damage)
        {
            ApplyDamageAndEffects();
            OnHealthUpdateChangeAppearance();
            StartCollisionCooldown();
        }

        public void ApplyKnockback()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            Vector3 pushDirection = -transform.forward;
            rb.AddForce(pushDirection * pushBackForce, ForceMode.VelocityChange);
        }

        // public GameObject bulletPrefab;
        // public Transform firePoint;
        // public float fireInterval = 3f;
        // public float bulletSpeed = 8f;

        // private void FireBullet()
        // {
        //     if (player != null)
        //     {
        //         // Instantiate the bullet at the fire point on the enemy
        //         GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        //         // Calculate the direction from the enemy to the player
        //         Vector3 directionToPlayer = (player.position - firePoint.position).normalized;

        //         // Set the bullet's velocity to move towards the player
        //         bullet.GetComponent<Rigidbody>().velocity = directionToPlayer * bulletSpeed;

        //         // Make the bullet smaller
        //         float bulletScale = 0.3f; // Adjust as needed
        //         bullet.transform.localScale = new Vector3(bulletScale, bulletScale, bulletScale);

        //         // Destroy the bullet after 2 seconds
        //         Destroy(bullet, 2f);
        //     }
        // }

        // private void OnTriggerEnter(Collider other)
        // {
        //     Debug.Log("Trigger entered with: " + other.name);

        //     if (other.CompareTag("Ammo"))
        //     {
        //         Debug.Log("Ignoring collision with ammo");
        //         Physics.IgnoreCollision(GetComponent<Collider>(), other, true);
        //     }
        // }
    #endregion

    void OnTriggerStay(Collider other)
    {
        // Debug.Log("Inside collision...");
    }

    void OnTriggerExit(Collider other)
    {
        // Debug.Log("Exiting collision...");
    }
}