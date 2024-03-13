using System.Collections;
using UnityEngine;

// --- Base Class --- ///
public class EnemyCube : MonoBehaviour
{
    #region Basic Variables
        public float healthPoints = 3f;
        public float timeDurationRespawn = 4f;
        public float deathCooldown = 1f;
        public float pushForce = 3f;
        public float friction = 2f;
        private bool isKnockedBack = false;
        private bool isCollisionCooldown = false;
        public float collisionCooldown = 0.1f;
        public float maxHeight = 15f;
        public float rotationSpeed = 500f;
    #endregion

    #region Enemy Save Original Size and Position for the Respawn
        private Vector3 initialPosition, originalScale;
        private Quaternion originalRotation;
        private Color originalColor;
    #endregion

    public AudioSource slimeJumpSound;

    protected virtual void Start()
    {
        slimeJumpSound = GetComponent<AudioSource>();
        InitializeOriginalValues();
        StartCoroutine(JumpRoutine());
    }
    
    protected virtual void Update(){}

    protected void OnTriggerEnter(Collider other)
    {
        if (IsWeaponCollisionValid(other))
        {
            ApplyDamageAndEffects();
            CheckHealthAndUpdateAppearance();
            KnockBack();
            StartCollisionCooldown();
        }
    }

    protected virtual bool IsWeaponCollisionValid(Collider other)
    {
        return other.gameObject.CompareTag("Weapon") && !isCollisionCooldown;
    }

    protected virtual void ApplyDamageAndEffects()
    {
        ApplyForce();
        healthPoints--;
    }

    protected virtual void CheckHealthAndUpdateAppearance()
    {
        switch (healthPoints)
        {
            case 2: UpdateAppearance(new Color32(170, 0, 0, 200), originalScale * 0.8f); break;
            case 1: UpdateAppearance(new Color32(70, 0, 0, 200), originalScale * 0.6f); break;
            case 0: UpdateAppearance(new Color32(10, 0, 0, 200), originalScale * 0.2f);
                    // Invoke(nameof(DestroyEnemy), deathCooldown); // destroy object after a certain time
                    StartCoroutine(RespawnEnemy()); // this is for testing purposes
                    break;
        }
    }

    protected virtual void StartCollisionCooldown()
    {
        isCollisionCooldown = true;
        Invoke(nameof(EndCollisionCooldown), collisionCooldown);
    }

    protected virtual void EndCollisionCooldown()
    {
        isCollisionCooldown = false;
    }

    protected virtual void DestroyEnemy()
    {
        gameObject.SetActive(false);
        print("Enemy Defeated!");
    }

    protected virtual void ApplyForce()
    {
        Vector3 pushDirection = -transform.forward;
        GetComponent<Rigidbody>().AddForce(pushDirection * pushForce, ForceMode.VelocityChange);
        isKnockedBack = true;
    }

    protected virtual void KnockBack()
    {
        if (isKnockedBack)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity -= friction * Time.deltaTime * rb.velocity;

            if (rb.velocity.magnitude < 0.1f)
            {
                rb.velocity = Vector3.zero;
                isKnockedBack = false;
            }
        }
        MaxHeightAfterHit();
    }

    protected virtual IEnumerator JumpRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            EnemyJump();
        }
    }

    protected virtual void EnemyJump()
    {
        if (healthPoints > 0)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * pushForce, ForceMode.VelocityChange);
            if (slimeJumpSound != null)
            {
                slimeJumpSound.Play();
            }
        }
    }

    protected virtual void MaxHeightAfterHit()
    {
        if (transform.position.y > maxHeight)
        {
            Vector3 newPos = transform.position;
            newPos.y = maxHeight;
            transform.position = newPos;
        }
    }

    protected virtual IEnumerator RespawnEnemy()
    {
        yield return new WaitForSeconds(timeDurationRespawn);
        UpdateAppearance(originalColor, originalScale);
        ResetEnemyState();
        gameObject.SetActive(true);
        print("Enemy Respawned!");
    }

    protected virtual void ResetEnemyState()
    {
        healthPoints = 3;
        transform.position = initialPosition;
        transform.rotation = originalRotation;
    }

    // Color
    protected virtual void UpdateAppearance(Color color, Vector3 scale)
    {
        GetComponent<MeshRenderer>().material.color = color;
        transform.localScale = scale;
    }

    protected virtual void InitializeOriginalValues()
    {
        initialPosition = transform.position;
        originalColor = GetComponent<MeshRenderer>().material.color;
        originalScale = transform.localScale;
        originalRotation = transform.rotation;
    }
    
    protected virtual void OnTriggerStay(Collider other)
    {
        Debug.Log("Inside collision...");
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        Debug.Log("Exiting collision...");
    }
}