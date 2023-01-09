using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator animator;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    

    private void Update() 
    {
        if(animator.GetNextAnimatorStateInfo(0).IsName("End"))
        {
            Destroy(gameObject);
        }
        //if(animator.Controller)
    }
}
