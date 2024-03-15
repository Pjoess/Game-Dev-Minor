using System.Collections;
using UnityEngine;

public partial class Enemy : MonoBehaviour, IDamageble
{   
    public void Update(){
        if (player != null) FacePlayer(); // Face the player
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

    public void OnTriggerEnter(Collider other)
    {
       if (IsWeaponCollisionValid(other))
       {
           ApplyDamageAndEffects();
           // CheckHealthAndUpdateAppearance();
       }
    }

    public void ApplyDamageAndEffects() => healthPoints--;

    public bool IsWeaponCollisionValid(Collider other)
    {
        return other.gameObject.CompareTag("Weapon") && !isCollisionCooldown;
    }

    

    public void CheckHealthAndUpdateAppearance()
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

    public void DestroyEnemy()
    {
        gameObject.SetActive(false);
        print("Enemy Defeated!");
    }

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
                slimeJumpSound.Play();
            }
        }
    }

    public IEnumerator RespawnEnemy()
    {
        yield return new WaitForSeconds(respawnTime);
        UpdateAppearance(originalColor, originalScale);
        ResetEnemyStatePosition();
        gameObject.SetActive(true);
        print("Enemy Respawned!");
    }

    public void ResetEnemyStatePosition()
    {
        healthPoints = 3f;
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

    public void Hit()
    {
        ApplyDamageAndEffects();
        CheckHealthAndUpdateAppearance();
        StartCollisionCooldown();
    }

    public void ApplyKnockback()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 pushDirection = -transform.forward;
        rb.AddForce(pushDirection * pushBackForce, ForceMode.VelocityChange);
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        // Debug.Log("Inside collision...");
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        // Debug.Log("Exiting collision...");
    }
}