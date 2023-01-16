using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public float rotationSpeed = 0.005f;

    public float wheelSpeed = 0.02f;

    public float arrowSpeed = 0.005f;

    public enum Control{X_Axis, MouseWheel, Arrows};

    public Control control;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(control == Control.X_Axis)
        {
            float rotation = Input.GetAxis("Mouse X") * rotationSpeed;

            transform.Rotate(0, 0, rotation);
        }
        if(control == Control.MouseWheel)
        {
            float rotation = Input.mouseScrollDelta.y * wheelSpeed;

            transform.Rotate(0, 0, rotation);
        }
        if(control == Control.Arrows)
        {
            float rotation = Input.GetAxisRaw("Horizontal") * arrowSpeed;

            transform.Rotate(0, 0, rotation);
        }
    }
}
