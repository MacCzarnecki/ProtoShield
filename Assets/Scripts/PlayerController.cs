using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxSpeed;
    public float jumpTakeOffSpeed;
    public bool grounded;

    public bool blockedRight => rb.IsTouching(RightFilter);

    public bool blockedLeft => rb.IsTouching(LeftFilter);
    public int maxHealth;
    public int currentHealth;
    public ContactFilter2D GroundFilter;

    public ContactFilter2D RightFilter;
    public ContactFilter2D LeftFilter;
    public CompositeCollider2D ground;

    public GameObject endGoal;

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
            hearts[i] = Instantiate(_heart, new Vector3(10.0f - 2.5f * i, 5.0f, 0.0f), Quaternion.identity);
            hearts[i].transform.parent = Camera.main.transform;
        }
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        grounded = rb.IsTouching(GroundFilter);
        for(int i = maxHealth - 1; i >= 0; i--)
        {
            if(currentHealth < i + 1)
                hearts[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/UI/Broken Heart") as Sprite;
            else
                hearts[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/UI/Heart") as Sprite;
        }
        if(currentHealth == 0) Die();

        if(SceneParameters.scene != MenuController.LoadedScene.Platform) return;

        if (Input.GetKeyDown("w") && grounded) {
            rb.AddForce(new Vector2(0.0f, jumpTakeOffSpeed), ForceMode2D.Impulse);
            grounded = false;
        }
        if (Input.GetKey("d") && !blockedRight) {
            rb.velocity = new Vector2(maxSpeed,rb.velocity.y);
        }
        else if(rb.velocity.x > 0.0f)
        {
            rb.velocity = new Vector2(rb.velocity.x/3*2, rb.velocity.y);
        }
        if (Input.GetKey("a") && !blockedLeft) {
            rb.velocity = new Vector2(-maxSpeed,rb.velocity.y);
        }
        else if(rb.velocity.x < 0.0f)
            rb.velocity = new Vector2(rb.velocity.x/3*2, rb.velocity.y);

    }

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
    
    public bool IsAtEnd()
    {
        return (transform.position - endGoal.transform.position).magnitude < 1.0f;
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
