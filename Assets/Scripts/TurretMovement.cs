using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMovement : MonoBehaviour
{   
    Vector3 direction;

    Vector3 offset;

    public GameObject player;
    public float speed;
    private float offsetMultiplier;
    // Start is called before the first frame update
    void Start()
    {
        offsetMultiplier = Random.Range(-0.02f, 0.02f);
        direction = new Vector3(Random.Range(-1.0f, 1.0f) ,Random.Range(-1.0f, 1.0f), 0.0f);
        direction.Normalize();
        offset = Vector3.Cross(direction, Vector3.back).normalized * offsetMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        direction += offset;
        direction.Normalize();
        offset = Vector3.Cross(direction, Vector3.back).normalized * offsetMultiplier;
        Vector3 newPosition = transform.position + direction * speed;
        isInCameraRange(newPosition.x, newPosition.y);
        if((newPosition - player.transform.position).magnitude < 3.0f)
        {
            direction = Vector3.Reflect(direction,transform.position - player.transform.position);
            direction.Normalize();
            offset = Vector3.Reflect(offset, direction);
            direction += offset;
            direction.Normalize();
            transform.position += direction * speed;
        }
        else
            transform.position += direction * speed;
    }

    void isInCameraRange(float x, float y)
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = Camera.main.orthographicSize + cameraPosition.y - .5f;
        float cameraWidth = screenAspect * Camera.main.orthographicSize + cameraPosition.x - .5f;
        Debug.Log(cameraWidth.ToString() + cameraHeight.ToString());
        if(y > cameraHeight || y <  -(cameraHeight))
        {
            direction.y = -direction.y;
        }
        if(x > cameraWidth || x < -(cameraWidth))
        {
            direction.x = -direction.x;
        }
        offset = Vector3.Cross(direction, Vector3.back).normalized * offsetMultiplier;
    }
}
