using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public float rotationSpeed = 0.005f;

    public float wheelSpeed = 0.02f;

    public enum Control{X_Axis, MouseWheel};

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
            float rotation = Input.mouseScrollDelta.y * rotationSpeed;

            transform.Rotate(0, 0, rotation);
        }

    }
}
