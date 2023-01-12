using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    // Start is called before the first frame update
   private LineRenderer bolt;

    private void Awake()
    {
        bolt = GetComponent<LineRenderer>();
    }
    
    private void FixedUpdate() {
        var color = bolt.material.color;
 
        bolt.material.color = new Color(color.r, color.g, color.b, color.a - 0.1f);
    }

    public void SetRenderer(Vector3 startPosition, Vector3 endPosition)
    {
        bolt.material = bolt.materials[0];
        bolt.positionCount = 2;
        bolt.SetPositions(new Vector3[]{startPosition, endPosition});
    }

    public bool isFaded()
    {   
        if(bolt.material.color.a <= 0.0f)
            return true;
        else
            return false;
    }
}
