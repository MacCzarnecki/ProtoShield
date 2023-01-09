using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    public int maxHealth;
    public int currentHealth;

    Animator animator;


    public GameObject heart;

    private GameObject[] hearts;
    void Awake()
    {
        currentHealth = maxHealth;
        hearts = new GameObject[maxHealth];
        for(int i = 0; i < hearts.Length; i++)
        {
            hearts[i] = Instantiate(heart, new Vector3(3.0f - 0.5f * i, 1.0f, 0.0f), Quaternion.identity);
        }
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate() {
        for(int i = maxHealth - 1; i >= 0; i--)
        {
            if(currentHealth < i + 1)
                hearts[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/UI/Broken Heart") as Sprite;
            else
                hearts[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/UI/Heart") as Sprite;
        }
        if(currentHealth == 0) Die();
    }
    // Update is called once per frame

    private void Die()
    {
        animator.Play("Death", 0);
        Destroy(gameObject, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    }

    public void TakeDamage()
    {
        currentHealth--;
    }
}
