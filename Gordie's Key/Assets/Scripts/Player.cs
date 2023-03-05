using System.Collections;
using UnityEngine;


public class Player : MonoBehaviour
{
    private float horizontal;
    private float speed = 3.5f;
    private float jumpingPower = 6f;
    private bool isFacingRight = true;

    public int maxHealth = 5;
    public int currentHealth;
    public HealthBar healthbar;
    public HealthBar EnemyHealthBar;
    

    //_______________________________________

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask EnemyLayers;

    public float attackRate = 2f;
    float nextAttacTime = 0f;

    //________________________________________
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool doubleJump;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(5f, 6f);

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 10f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;


    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private GameObject middlecheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private TrailRenderer tr;

    public SpriteRenderer sprite;

    private Animator animator;

    public GameObject ladders;

    public CameraManager camMngr;

    public GameObject DeadScene;



    private void Start()
    {
        Time.timeScale = 1;
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);

        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (isDashing)
        {
            return;
        }

        if (Time.time>=nextAttacTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                nextAttacTime = Time.time + 1f / attackRate;
            }
        }
        
        

        horizontal = Input.GetAxisRaw("Horizontal");
        
        if (IsGrounded() && !Input.GetKey(KeyCode.W))
        {
            doubleJump = false;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (IsGrounded() || doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

                doubleJump = !doubleJump;
            }
        }
        if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetKeyUp(KeyCode.W) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        WallSlide();
        WallJump();
        
        
        if (horizontal > 0 || horizontal < 0)
        {
            animator.SetFloat("SPEED", 1);
        }
        else
        {
            animator.SetFloat("SPEED", 0);
        }

        

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }


        

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        if (!isWallJumping)
        {
            Flip();
        }

        
    }

    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthbar.SetHealth(currentHealth);

        if (currentHealth > 0)
        {
            animator.SetTrigger("Hurt");
        }
        else
        {
            animator.SetTrigger("Die");
            StartCoroutine(DeadSceneComing());
            
        }
    }

    IEnumerator DeadSceneComing()
    {
        yield return new WaitForSeconds(1);
        DeadScene.SetActive(true);
        Time.timeScale = 0;
    }


    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, EnemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.tag== "BossEnemy")
            {
                enemy.GetComponent<FinalBossAI>().TakeDamageBoss(2);
            }
            else if (enemy.gameObject.tag == "LittleEnemy")
            {                
                enemy.GetComponent<SwordEnemyAI>().TakeDamageEnemy(2);
            }
        }
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ladders!=null&& camMngr!=null)
        {
            if (collision.gameObject.tag == "OpenEnemyHealthBar")
            {
                EnemyHealthBar.gameObject.SetActive(true);
            }
            else if (collision.gameObject.tag == "OpenTheLadders")
            {
                ladders.SetActive(true);
            }
            else if (collision.gameObject.tag == "ChangeCameraPoses")
            {
                camMngr.minY = 15f;
                camMngr.maxY = 20f;
            }
        }
        
        
        
    }





    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }


    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }
    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.W) && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }



    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalgravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalgravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

}