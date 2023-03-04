using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossAI : MonoBehaviour
{
    Animator EnemyAnimator;

    private void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        EnemyAnimator.SetFloat("Speed", Mathf.Abs(5.0f));
    }
}