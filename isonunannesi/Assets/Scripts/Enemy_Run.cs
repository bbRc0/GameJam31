using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Run : MonoBehaviour
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

    public Transform target;
    public float AIspeed;
    public float StopDistance;
    private bool IsChasing;

    

    private void Awake()
    {
        anim = GetComponent<Animator>();
        
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
        if (IsChasing)
        {
            EnemyFollow();
        }
        else
        {
            if (Vector2.Distance(transform.position, target.position) > StopDistance)
            {
                IsChasing = true;
            }
        }


            

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
    private void EnemyFollow()
    {
        
            if (transform.position.x > target.transform.position.x)
            {
                transform.position += Vector3.left * AIspeed * Time.deltaTime;
            }
            if (transform.position.x < target.transform.position.x)
            {
                transform.position += Vector3.right * AIspeed * Time.deltaTime;
            }
        
    }
}
