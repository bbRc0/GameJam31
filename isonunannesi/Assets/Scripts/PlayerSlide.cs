using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlide : MonoBehaviour
{
    public bool isSliding = false;
   
    public Player pl;
    public Rigidbody2D rigidbody;

    public Animator anim;

    public BoxCollider2D regularColl;
    public BoxCollider2D sliderColl;

    public float slideSpeed = 5f;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            prefromSlide();
        }
    }

    void prefromSlide()
    {
        isSliding = true;

        anim.SetBool("isSlide", true);

        regularColl.enabled = false;
        sliderColl.enabled = true;

        if (!pl.sprite.flipX)
        {
            rigidbody.AddForce(Vector2.right * slideSpeed);
        }
        else
        {
            rigidbody.AddForce(Vector2.left * slideSpeed);
        }
        StartCoroutine("stopSlide");
    }
    IEnumerator stopSlide()
    {
        yield return new WaitForSeconds(0.8f);
        anim.Play("Idle");
        anim.SetBool("IsSlide", false);
    }
}