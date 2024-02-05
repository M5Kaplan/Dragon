using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;


public class DragonMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 1f;
    [SerializeField] float JumpPower = 1f;
    [SerializeField] float coolDown = 1f;
    [SerializeField] int coinCount = 0;
    Vector2 StartingPosition;

    bool isAlive = true;
    //bool canFire = true;
    bool canShoot = false;

    Vector2 moveInput;
    [SerializeField] GameObject FireBall;
    [SerializeField] Transform firePosition;
    [SerializeField] GameObject uiSymbol;
    [SerializeField] TMP_Text coinCountText;



    Rigidbody2D rb;
    BoxCollider2D myBody;
    CapsuleCollider2D myFeet;
    Animator myAnimator;
    List<Slime> slime = new List<Slime>();
    DeathUI deathUI;

    [SerializeField] AudioClip pickupCoin;
    [SerializeField] AudioClip fireballshoot;
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip slimeJump;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myBody = GetComponent<BoxCollider2D>();
        myFeet = GetComponent<CapsuleCollider2D>();
        myAnimator = GetComponent<Animator>();
        StartingPosition = transform.position;
        deathUI = FindAnyObjectByType<DeathUI>();

        slime.AddRange(FindObjectsOfType<Slime>());

    }

    void Update()
    {
        Run();
        Flip();
        Die();
        SlimeJump();
        uiFire();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            coinCount++;
            UpdateCoinUI();
            Destroy(other.gameObject);
        }
    }
    void UpdateCoinUI()
    {
        AudioSource.PlayClipAtPoint(pickupCoin, Camera.main.transform.position);
        coinCountText.text = "Coins: " + coinCount.ToString();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        if (value.isPressed && myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            AudioSource.PlayClipAtPoint(jump, Camera.main.transform.position);
            rb.velocity += new Vector2(0f, JumpPower);
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive ||!canShoot) { return; }
        StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        
        canShoot = false;
        myAnimator.SetTrigger("Attack");

        yield return new WaitForSeconds(.3f);
        AudioSource.PlayClipAtPoint(fireballshoot, Camera.main.transform.position);
        Instantiate(FireBall, firePosition.position, transform.rotation);

        yield return new WaitForSeconds(coolDown);
        
       
    }

    void Run()
    {
        if (!isAlive) { return; }

        if (CanMove)
        {
            Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, rb.velocity.y);
            rb.velocity = playerVelocity;

            bool PlayerSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
            if (myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                myAnimator.SetBool("IsWalking", PlayerSpeed);
            }
        }
        if (!CanMove)
        {
            rb.velocity = new Vector2(0, 0);
        }
    }
    void Die()
    {
        if (myBody.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            isAlive = false;
            SceneManager.LoadScene(0);
            myAnimator.SetBool("isAlive", false);
            myAnimator.SetBool("canMove", false);
            rb.velocity = Vector2.zero;
            moveInput = Vector2.zero;
            //Invoke("Revive", 1f);
            
            
        }
    }
    void Revive()
    {
        SceneManager.LoadScene(0);
        /*isAlive = true;
        myAnimator.SetBool("isAlive", true);
        myAnimator.SetBool("canMove", true);
        transform.position = StartingPosition; */

    }

    void Flip()
    {
        bool PlayerSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        if (PlayerSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(-rb.velocity.x), 1f);
        }
    }

    public bool CanMove
    {
        get
        {
            return myAnimator.GetBool("canMove");
        }
    }

    public void SlimeJump()
    {
        if (myFeet.IsTouchingLayers(LayerMask.GetMask("Bounce")))
        {
            AudioSource.PlayClipAtPoint(slimeJump, Camera.main.transform.position);
            canShoot = true;
            Debug.Log("Temas ettik");
            Slime[] allSlimes = FindObjectsOfType<Slime>();
            foreach (Slime currentSlime in allSlimes)
            {
                if (myFeet.IsTouching(currentSlime.GetComponent<Collider2D>()))
                {
                    currentSlime.BounceAnimation();
                    break;
                }
            }
        }
    }
    void uiFire()
    {
        if (canShoot)
        {
            uiSymbol.SetActive(true);
        }
        else
        {
            uiSymbol.SetActive(false);
        }
    }
}


