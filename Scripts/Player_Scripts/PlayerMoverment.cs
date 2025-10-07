using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int facingDirection = 1;

    private Rigidbody2D rb;
    public Animator anim;

    private bool isKnockedback;

    public NewBehaviourScript playerCombat; // Sửa tên class

    private void Update()
    {
        if (Input.GetButtonDown("normalAttack"))
        {
            playerCombat.Attack(); // Sửa tên biến
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (isKnockedback == false)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Flip khi di chuyển
            if (horizontal > 0) // Đi phải
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                facingDirection = 1;
            }
            else if (horizontal < 0) // Đi trái
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                facingDirection = -1;
            }

            anim.SetFloat("horizontal", Mathf.Abs(horizontal));
            anim.SetFloat("vertical", Mathf.Abs(vertical));
            rb.velocity = new Vector2(horizontal, vertical) * StatsManager.Instance.speed;
        }
    }

    public void Knockback(Transform damageSource, float knockbackForce, float knockbackDuration)
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        StartCoroutine(KnockbackRoutine(damageSource, knockbackForce, knockbackDuration));
    }

    private IEnumerator KnockbackRoutine(Transform damageSource, float knockbackForce, float knockbackDuration)
    {
        Vector2 direction = (transform.position - damageSource.position).normalized;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);
    }

    IEnumerator KnockbackCounter(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        rb.velocity = Vector2.zero;
        isKnockedback = false;
    }
}