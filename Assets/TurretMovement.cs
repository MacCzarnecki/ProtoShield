using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMovement : MonoBehaviour
{   
    Vector3 direction;

    Vector3 offset;

    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        direction = new Vector3(Random.Range(-1.0f, 1.0f) ,Random.Range(-1.0f, 1.0f), 0.0f);
        direction.Normalize();
        offset = Vector3.Cross(direction, Vector3.back) * Random.Range(-0.2f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        offset += offset.normalized * Random.Range(-0.1f, 0.1f);
        Vector3 newDirection = direction + offset;
        newDirection.Normalize();
        Vector3 newPosition = transform.position + newDirection * speed;
        if(isInCameraRange(newPosition.x, newPosition.y))
        {
            direction += offset;
            direction.Normalize();
            transform.position += direction * speed;
        }
        else
        {
            Debug.Log("Hit Bound");
            offset = -offset;
            direction = -direction;
            direction += offset;
            direction.Normalize();
            transform.position += direction * speed;
        }
    }

    bool isInCameraRange(float x, float y)
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = Camera.main.orthographicSize + cameraPosition.y;
        float cameraWidth = screenAspect * Camera.main.orthographicSize + cameraPosition.x;
        if(y > cameraHeight || y <  -cameraHeight)
            return false;
        if(x > cameraWidth || x < -cameraWidth)
            return false;
        return true;
    }
}
