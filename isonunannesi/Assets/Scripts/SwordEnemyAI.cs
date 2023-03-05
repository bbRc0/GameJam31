using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEnemyAI : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    private Player playerscript;

    private Animator anim;
    private EnemyPatrol enemyPatrol;

    public int maxHealth = 15;
    public int currentHealth;
    public HealthBar healthbar;
    
    

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        
    }
    private void Start()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("Attack");
            }
        }
        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }
    private bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        { 
            playerscript = hit.collider.gameObject.GetComponent<Player>(); 
        }

        return hit.collider != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            playerscript.TakeDamage(damage);
        }

    }
    public void TakeDamageEnemy (int damage)
    {
        currentHealth -= damage;
        healthbar.SetHealth(currentHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("Hurt");
        }
        else
        {
            anim.SetTrigger("Die");
            StartCoroutine(WaitForTwoSeconds());
            
        }
    }

    IEnumerator WaitForTwoSeconds()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        healthbar.gameObject.SetActive(false);
        
    }
}
