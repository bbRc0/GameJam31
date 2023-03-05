using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossAI : MonoBehaviour
{
    Animator EnemyAnimator;
	public Transform middleCheck;

	public bool isFlipped = false;

	public int maxHealth =15;
	public int currentHealth;
	public HealthBar healthbar;

	

	private void Start()
    {
        EnemyAnimator = GetComponent<Animator>();

		currentHealth = maxHealth;
		healthbar.SetMaxHealth(maxHealth);

		
	}

    private void Update()
    {
        EnemyAnimator.SetFloat("Speed", Mathf.Abs(1.0f));
		LookAtPlayer();

    }
	public void LookAtPlayer()
	{
		Vector3 flipped = transform.localScale;
		flipped.z *= -1f;

		if (transform.position.x < middleCheck.position.x && isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = false;
		}
		else if (transform.position.x > middleCheck.position.x && !isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = true;
		}
	}
	public void TakeDamageBoss(int damage)
	{
		currentHealth -= damage;
		healthbar.SetHealth(currentHealth);

		if (currentHealth > 0)
		{
			 EnemyAnimator.SetTrigger("Hit"); 
		}
		else
		{
			 EnemyAnimator.SetTrigger("Die");  
			
		}
	}

}