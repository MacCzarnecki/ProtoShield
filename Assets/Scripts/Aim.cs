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

    RaycastHit2D[] hits;

    public float countdownStart;
    
    // Start is called before the first frame update

    private void Awake() 
    {
        animator = GetComponent<Animator>();
        countdownStart = Time.time;
    }

    private void FixedUpdate() {
        if(animator.GetCurrentAnimatorClipInfo(0).Length == 0 || animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Teleporting")
            Destroy(gameObject, 2.0f);
        else
        {
            Vector2 direction = player.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(Vector3.back, direction);
            hits = Physics2D.RaycastAll(transform.position, direction, direction.magnitude);
            //Debug.DrawRay(transform.position, direction, Color.green);

            if(animator.GetCurrentAnimatorClipInfo(0).Length == 0 || animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Loading")
            {
                LeadLazer(direction, false);
            }
            if(animator.GetCurrentAnimatorClipInfo(0).Length == 0 || animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Ready")
            {
                LeadLazer(direction, true);
            }
        }
    }

    private void LeadLazer(Vector2 direction, bool blink)
    {
        if(!blink)
        foreach(RaycastHit2D hit in hits)
            if(hit.collider == player.GetComponentInChildren<PolygonCollider2D>())
            {   
                Vector2 lazer = hit.point - new Vector2(transform.position.x, transform.position.y);
                Debug.DrawRay(transform.position, lazer, Color.red);
                onBlock = true;
                break;
            }
            else
            {
                Debug.DrawRay(transform.position, direction, Color.green);
                onBlock = false;
            }
    
        else
        {
            float frame = animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f;
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 4.0f)
            {
                player.GetComponent<PlayerController>().TakeDamage();
                animator.Play("Firing", 0);
                lightning = Instantiate(_lightning, Vector3.zero, Quaternion.identity);
                lightning.GetComponent<Lazer>().SetRenderer(player.transform.position, transform.position);
            }
            foreach(RaycastHit2D hit in hits)
            if(frame >= 0.5f)
            {
                if(hit.collider == player.GetComponentInChildren<PolygonCollider2D>())
                {   
                    Vector2 lazer = hit.point - new Vector2(transform.position.x, transform.position.y);
                    Debug.DrawRay(transform.position, lazer, Color.white);
                    onBlock = true;
                    break;
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
                    break;
                }
                else
                {
                    Debug.DrawRay(transform.position, direction, Color.green);
                    onBlock = false;
                }
            }
        }
    }

    public Vector3 GetHitPoint()
    {
        foreach(RaycastHit2D hit in hits)
            if(hit.collider == player.GetComponentInChildren<PolygonCollider2D>())
                return hit.point;
        return Vector3.zero;
    }
    public void Shoot()
    {
        animator.Play("Firing", 0);
        lightning = Instantiate(_lightning, Vector3.zero, Quaternion.identity);
        foreach(RaycastHit2D hit in hits)
            if(hit.collider == player.GetComponentInChildren<PolygonCollider2D>())
                lightning.GetComponent<Lazer>().SetRenderer(hit.point, transform.position);
    }

    public void onDestroy()
    {
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
