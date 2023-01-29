using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    public GameObject player;
    public GameObject Spark;
    private Animator animator;
    public bool onBlock = false;

    private LineRenderer line;
    
    public GameObject _lightning;
    private GameObject lightning;

    RaycastHit2D[] hits;

    public float countdownStart;
    
    // Start is called before the first frame update

    private void Awake() 
    {
        line = GetComponent<LineRenderer>();
        line.SetPosition(0, transform.position);
        line.SetPosition(1, player.transform.position);
        animator = GetComponent<Animator>();
        countdownStart = Time.time;
    }

    private void FixedUpdate() {
        if(animator.GetCurrentAnimatorClipInfo(0).Length == 0 || animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Teleporting")
            Destroy(gameObject, 2.0f);
        if(player == null) return;
        Vector2 direction = player.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.back, direction);
        hits = Physics2D.RaycastAll(transform.position, direction, direction.magnitude);
            //Debug.DrawRay(transform.position, direction, Color.green);

        LeadLazer();
    }

    private void LeadLazer()
    {
        line.SetPosition(0, transform.position);
        foreach(RaycastHit2D hit in hits)
            if(hit.collider == player.GetComponentInChildren<PolygonCollider2D>())
            {  
                line.SetPosition(1, hit.point);
                line.startColor = line.endColor = Color.red;
                onBlock = true;
                break;
            }
            else
            {
                line.SetPosition(1, player.transform.position);
                line.startColor = line.endColor = Color.green;
                onBlock = false;
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
        line.enabled = false; 
        foreach(RaycastHit2D hit in hits)
            if(hit.collider == player.GetComponentInChildren<PolygonCollider2D>())
                lightning.GetComponent<Lazer>().SetRenderer(hit.point, transform.position);
    }

    public string AnimationState()
    {
        return animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }
}
