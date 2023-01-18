using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    // Start is called before the first frame update
   private LineRenderer bolt;

   public Texture[] LazerFade;

    private void Awake()
    {
        bolt = GetComponent<LineRenderer>();
    }

    int index = 0;
    
    private void FixedUpdate() {
        bolt.material.SetTexture("_MainTex", LazerFade[index]);
        index++;
        if(index >= LazerFade.Length)
            Destroy(gameObject);
    }

    public void SetRenderer(Vector3 startPosition, Vector3 endPosition)
    {
        bolt.positionCount = 2;
        bolt.SetPositions(new Vector3[]{startPosition, endPosition});
    }
}
