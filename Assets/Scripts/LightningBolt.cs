using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : MonoBehaviour
{
    private LineRenderer bolt;

    private void Awake()
    {
        bolt = GetComponent<LineRenderer>();
    }
    
    private void FixedUpdate() {
        var color = bolt.material.color;
 
        bolt.material.color = new Color(color.r, color.g, color.b, color.a - 0.05f);
    }

    public void SetRenderer(Vector3 playerPosition, Vector3 collisionPosition, bool isBloom)
    {
        if(isBloom)
        {
            bolt.material = bolt.materials[1];
            bolt.material.color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
            bolt.positionCount = 2;
            bolt.SetPositions(new Vector3[]{playerPosition, collisionPosition});
            bolt.startWidth = 1.0f;
            bolt.endWidth = 0.8f;
            //bolt.numCapVertices = 16;
            return;
        }
        
        Vector3[] breakPoints = new Vector3[Random.Range(8, 12)];

        Vector3 tangent = collisionPosition - playerPosition;
        Vector3 normal = Vector3.Cross(tangent, Vector3.back);
        normal = Vector3.Normalize(normal);
        
        float length = breakPoints.Length - 2;

        breakPoints[0] = playerPosition;

        for(int i = 1; i < length + 1; i++)
            breakPoints[i] = (breakPoints[i - 1] + tangent / length * Random.Range(0.90f, 1.05f));

        breakPoints[(int)length + 1] = collisionPosition;

        for (int i = 1; i < length + 1; i++)
        {
            breakPoints[i] += normal * Random.Range(-1.0f, 1.0f) * 0.1f;
        }

        bolt.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        bolt.positionCount = breakPoints.Length;
        bolt.SetPositions(breakPoints);
    }

    public bool isFaded()
    {   
        if(bolt.material.color.a <= 0.0f)
            return true;
        else
            return false;
    }
}