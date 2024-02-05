using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] bool canMove = true;

    public DetectionZone attackZone;
    

    Rigidbody2D rb;
    Animator anim;
    CapsuleCollider2D sword;

    public bool isAlive = true;
    public bool _hasTarget = false;
  

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sword = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        Walk();
        HasTarget = attackZone.detectedColliders.Count > 0;
    }
    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {

            _hasTarget = value;
            anim.SetBool("hasTarget", value);
            if (value)
            {
                Invoke("EnableSword", 0.2f);
            }
            else
            {
                sword.enabled = false;
            }
        }
    }
    void EnableSword()
    {
        sword.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "turnPoint":
                moveSpeed = -moveSpeed;
                transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), 1f);
                break;
            case "FireBall":
                Destroy(gameObject);
                break;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        string tag = other.gameObject.tag;
        if (other.gameObject.CompareTag("Fireball"))
        {

            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        
        yield return new WaitForSeconds(.1f);
        Destroy(gameObject);
    }

    void Walk()
    {
        if (CanMove && canMove)
        {
            rb.velocity = new Vector2(moveSpeed, 0);
            anim.SetBool("Walk", true);
        }
        if(!CanMove&& !canMove)
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, 0.5f), 0);

        }
    }

    public bool CanMove
    {
        get
        {
            return anim.GetBool("canMove");
        }
    }

    }