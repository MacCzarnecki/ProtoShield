using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackGroundSlide : MonoBehaviour {
    public float scrollSpeed;


    void Update () {
        float newPos = Mathf.Repeat(Time.time * scrollSpeed, 9.6f);
        transform.position = Vector3.zero + new Vector3(0.1f, 0.1f, 0.0f)* newPos;
    }
}