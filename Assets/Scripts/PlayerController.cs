using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    public bool grounded => rb.IsTouching(ContactFilter);
    public int maxHealth;
    public int currentHealth;

    public ContactFilter2D ContactFilter;
    public CompositeCollider2D ground;

    RaycastHit2D hit;
    Animator animator;
    public GameObject _heart;

    Rigidbody2D rb;

    BoxCollider2D box;
    private GameObject[] hearts;
    void Awake()
    {
        currentHealth = maxHealth;
        hearts = new GameObject[maxHealth];
        for(int i = 0; i < hearts.Length; i++)
        {
            hearts[i] = Instantiate(_heart, new Vector3(14.0f - 2.5f * i, 5.0f, 0.0f), Quaternion.identity);
        }
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        for(int i = maxHealth - 1; i >= 0; i--)
        {
            if(currentHealth < i + 1)
                hearts[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/UI/Broken Heart") as Sprite;
            else
                hearts[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/UI/Heart") as Sprite;
        }
        if(currentHealth == 0) Die();

        if (Input.GetKeyDown("w") && grounded) {
            rb.velocity = new Vector2(rb.velocity.x,jumpTakeOffSpeed);
        }
        if (Input.GetKey("d")) {
            rb.velocity = new Vector2(2.0f,rb.velocity.y);
        }
        if (Input.GetKey("a")) {
            rb.velocity = new Vector2(-2.0f,rb.velocity.y);
        }
        //ComputeVelocity (); 
    }

    /*private void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis ("Horizontal");

        if (Input.GetButtonDown ("Jump") && grounded) {
            velocity.y = jumpTakeOffSpeed;
        } else if (Input.GetButtonUp ("Jump")) 
        {
            if (velocity.y > 0) {
                velocity.y = velocity.y * 0.5f;
            }
        }

        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
        if (flipSprite) 
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        animator.SetBool ("grounded", grounded);
        animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }*/
    private void FixedUpdate() 
    {
        
    }
    // Update is called once per frame

    private void Die()
    {
        animator.Play("Death", 0);
        transform.GetChild(0).gameObject.SetActive(false);
        Destroy(gameObject, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    }

    void OnDestroy() {
        foreach(var heart in hearts)
            Destroy(heart);
    }

    public void TakeDamage()
    {
        currentHealth--;
    }
}
