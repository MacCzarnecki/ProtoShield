using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(PolygonCollider2D))]
public class DrawCircle : MonoBehaviour
{
    public float currentRotation;

    public LineRenderer circleRenderer;
    // Start is called before the first frame update
    public int degrees;
    public float radius;
    PolygonCollider2D polygonCollider;

    float BG = 1.0f;
    GameObject[] enemies;

    void Awake() {
        degrees = 60;
        polygonCollider = GetComponent<PolygonCollider2D>();
    }
    void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        drawCircle();
        setCollider();
        circleRenderer.material.color = new Color(1.0f, BG, BG);
        if(BG < 1.0f)
            BG += 0.01f;
            
    }

    public void Reflect()
    {
        BG = 0.3f;
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

        }
    }

    void setCollider()
    {
        Vector3[] positions = new Vector3[circleRenderer.positionCount/5 + 1];

        for(int i = 0; i < circleRenderer.positionCount; i+=5)
            positions[(i + 1)/5] = circleRenderer.GetPosition(i);

        positions[positions.Length - 1] = circleRenderer.GetPosition(circleRenderer.positionCount - 1); 
        
        polygonCollider.pathCount = positions.Length - 1;

        float width = circleRenderer.startWidth;

        for(int i = 0; i < positions.Length - 1; i++)
        {
            float m = (positions[i + 1].y - positions[i].y) / (positions[i + 1].x - positions[i].x);
            float deltaX = (width / 2f) * (m / Mathf.Pow(m * m + 1, 0.5f));
            float deltaY = (width / 2f) * (1 / Mathf.Pow(1 + m * m, 0.5f));

            Vector3[] offsets = new Vector3[2];
            offsets[0] = new Vector3(-deltaX, deltaY);
            offsets[1] = new Vector3(deltaX, -deltaY);

            List<Vector2> colliderPoints = new List<Vector2>
            {
                positions[i] + offsets[0],
                positions[i + 1] + offsets[0],
                positions[i + 1] + offsets[1],
                positions[i] + offsets[1]
            };

            polygonCollider.SetPath(i, colliderPoints.ConvertAll(p => (Vector2)transform.InverseTransformPoint(p)));
        }
    }
}
