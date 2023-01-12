using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    public GameObject player;
    public GameObject Spark;
    private Animator animator;
    public bool onBlock = false;

    public GameObject _lightning;
    private GameObject lightning;

    RaycastHit2D hit;

    public float countdownStart;
    
    // Start is called before the first frame update

    private void Awake() 
    {
        animator = GetComponent<Animator>();
        //countdownStart = 0.0f;
    }

    private void FixedUpdate() {
        if(animator.GetCurrentAnimatorClipInfo(0).Length == 0 || animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Teleporting")
        {
            countdownStart = 0.0f;
            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            Vector2 direction = player.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(Vector3.back, direction);
            hit = Physics2D.Raycast(transform.position, direction, direction.magnitude);
            //Debug.DrawRay(transform.position, direction, Color.green);

            if(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Loading")
            {
                LeadLazer(direction, false);
            }
            else if(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Ready")
            {
                if(countdownStart == 0.0f)
                    countdownStart = Time.time;
                LeadLazer(direction, true);
            }
        }
    }

    private void LeadLazer(Vector2 direction, bool blink)
    {
        if(!blink)
            if(hit.collider == player.GetComponentInChildren<PolygonCollider2D>())
            {   
                Vector2 lazer = hit.point - new Vector2(transform.position.x, transform.position.y);
                Debug.DrawRay(transform.position, lazer, Color.red);
                onBlock = true;
            }
            else
            {
                Debug.DrawRay(transform.position, direction, Color.green);
                onBlock = false;
            }
        else
        {
            float frame = animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f;
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 8.0f)
            {
                player.GetComponent<PlayerController>().TakeDamage();
                animator.Play("Firing", 0);
                lightning = Instantiate(_lightning, Vector3.zero, Quaternion.identity);
                lightning.GetComponent<Lazer>().SetRenderer(player.transform.position, transform.position);
            }

            if(frame >= 0.5f)
            {
                if(hit.collider == player.GetComponentInChildren<PolygonCollider2D>())
                {   
                    Vector2 lazer = hit.point - new Vector2(transform.position.x, transform.position.y);
                    Debug.DrawRay(transform.position, lazer, Color.white);
                    onBlock = true;
                }
                else
                {
                    Debug.DrawRay(transform.position, direction, Color.white);
                    onBlock = false;
                }
            }
            else
            {
                if(hit.collider == player.GetComponentInChildren<PolygonCollider2D>())
                {   
                    Vector2 lazer = hit.point - new Vector2(transform.position.x, transform.position.y);
                    Debug.DrawRay(transform.position, lazer, Color.red);
                    onBlock = true;
                }
                else
                {
                    Debug.DrawRay(transform.position, direction, Color.green);
                    onBlock = false;
                }
            }

            if(lightning != null)
                if(lightning.GetComponent<LightningBolt>().isFaded())
                    Destroy(lightning);
        }
    }

    public Vector3 GetHitPoint()
    {
        return hit.point;
    }
    public void Shoot()
    {
        animator.Play("Firing", 0);
        lightning = Instantiate(_lightning, Vector3.zero, Quaternion.identity);
        lightning.GetComponent<Lazer>().SetRenderer(hit.point, transform.position);
    }

    public void onDestroy()
    {
        Instantiate<GameObject>(Spark, hit.point, transform.rotation);
        Destroy(gameObject);
    }

    public string AnimationState()
    {
        return animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }
    public float returnTime()
    {
        return Time.time - countdownStart;   
    }
}
