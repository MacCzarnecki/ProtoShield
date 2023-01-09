using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class DrawMenuCircle : MonoBehaviour
{
    // Start is called before the first frame update
    public float currentRotation;

    public LineRenderer circleRenderer;
    // Start is called before the first frame update
    public int degrees;
    public float radius;

    public Color currentColorStart;
    public Color currentColorEnd;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        setColor();
        drawCircle();
    }

    void drawCircle()
    {
        circleRenderer.positionCount = degrees;
        for(int currentStep = 0; currentStep<degrees ; currentStep++)
        {

            float currentRadian = (float)currentStep/360;

            currentRotation = transform.rotation.eulerAngles.z / 360;

            float xScaled = Mathf.Cos((currentRadian - currentRotation) * 2 * Mathf.PI);
            float yScaled = Mathf.Sin((currentRadian - currentRotation) * 2 * Mathf.PI);

            float x = transform.position.x + xScaled * radius;
            float y = transform.position.y + yScaled * radius;

            Vector3 currentPosition = new Vector3(x, y, 0);

            circleRenderer.SetPosition(currentStep, currentPosition);
            circleRenderer.startColor = currentColorStart;
            circleRenderer.endColor = currentColorEnd;
        }
    }

    void setColor()
    {
        currentColorStart = new Color(Mathf.Sin(currentRotation * 2 * Mathf.PI - 0.75f * Mathf.PI), Mathf.Sin(currentRotation * 2 * Mathf.PI + 0.25f * Mathf.PI), Mathf.Sin(currentRotation * 2 * Mathf.PI - 0.25f * Mathf.PI));
        currentColorEnd = new Color(Mathf.Sin((currentRotation - degrees/360.0f) * 2 * Mathf.PI - 0.75f * Mathf.PI), Mathf.Sin((currentRotation - degrees/360.0f) * 2 * Mathf.PI + 0.25f * Mathf.PI), Mathf.Sin((currentRotation - degrees/360.0f) * 2 * Mathf.PI - 0.25f * Mathf.PI));
    }
}
