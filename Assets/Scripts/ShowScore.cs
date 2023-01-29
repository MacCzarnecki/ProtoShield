using System.Collections;
using System;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowScore : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text field;
    public Canvas canvas;

    public GameObject ContinueButton;
    public GameObject QuitButton;
    private string text = "Score\n";
    void Start()
    {
        GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        //field = GetComponent<TMP_Text>();
    }
    private void Awake() {
        text = "Score\n";
        if(!SceneParameters.isDemo || SceneParameters.ShieldDegrees == 15)
            ContinueButton.SetActive(false);
    }
    public void RenderText(List<Scheduler.Json> _list)
    {
        for(int i = 0; i < _list.Count; i++)
        {
            text += String.Format("<cspace=0.1em>{0,2}.</cspace> Angle: <cspace=0.1em>{1,5:F2}°</cspace> Time: <cspace=0.1em>{2:F5}</cspace> s\n",(i + 1) , _list[i].angle, _list[i].time);
        }  
        
    }
    // Update is called once per frame
    void Update()
    {
        field.text = text;
        Vector2 direction =  Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.transform.position;
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), direction, direction.magnitude);
            if(hit.transform != null && hit.transform.gameObject == QuitButton)
            {
                if(Input.GetMouseButtonDown(0))
                    SceneManager.LoadScene("Menu");
                //endMenu.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/UI/Quit Button (Selected)") as Sprite;
                QuitButton.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/UI/Quit Button (Selected)") as Sprite;
            }
            else
                QuitButton.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/UI/Quit Button") as Sprite;

            if(hit.transform != null && hit.transform.gameObject == ContinueButton)
            {
                if(Input.GetMouseButtonDown(0))
                     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                //endMenu.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/UI/Quit Button (Selected)") as Sprite;
                ContinueButton.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/UI/Continue Button (Selected)") as Sprite;
            }
            else
                ContinueButton.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/UI/Continue Button") as Sprite;
            return;
    }
}
