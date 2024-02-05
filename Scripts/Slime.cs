using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float xScale = 0.4f;
    [SerializeField] float yScale = 0.4f;

    Rigidbody2D rb;
    public Animator anim;
   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        transform.localScale = new Vector2(Mathf.Sign(moveSpeed) * xScale, yScale);
        
    }


    void Update()
    {
        Walk();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "turnPoint":
                moveSpeed = -moveSpeed;
                transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x))*xScale, yScale);
                break;
        }

    }
    void Walk()
    {
        rb.velocity = new Vector2(moveSpeed, 0);

    }

    public void BounceAnimation()
    {
        Debug.Log("ulaaaan");
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        anim.SetTrigger("isBounced");
        
        yield return new WaitForSeconds(.5f);

        Destroy(gameObject);
    }

}
