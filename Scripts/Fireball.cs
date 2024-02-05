using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float Fireballspeed;
    [SerializeField] float xFire = 1f;

    DragonMovement player;
    Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<DragonMovement>();
        animator = GetComponent<Animator>();

        xFire = -player.transform.localScale.x * Fireballspeed;
    }

    void Update()
    {
        rb.velocity = new Vector2(xFire, 0);
        FlipBall();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        xFire = 0;       
       StartCoroutine(Hit());
        
    }
    IEnumerator Hit()
    {
        animator.SetTrigger("Hit");      
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject, .5f);
    }

    void FlipBall()
    {

        if (xFire < 0)
        {
            Debug.Log("sola bakýyom");
            animator.SetBool("Mirror", true);
            transform.rotation = Quaternion.Euler(0f, 0f, -180f);
        }
        else
        {
            Debug.Log("saða bakýyom");
            animator.SetBool("Mirror", false);
        }
    }


}
