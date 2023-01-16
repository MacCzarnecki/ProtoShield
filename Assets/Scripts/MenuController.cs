using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public enum Control{Selected, NotSelected};
    // Start is called before the first frame update
    public LayerMask mask;
    RaycastHit2D hit;

    public GameObject player;
    public Camera cam;

    private Animator animator;

    public Dictionary<GameObject, Control> buttons = new Dictionary<GameObject, Control>();
    
    private GameObject selectedButton;


    void Start()
    {
        animator = GetComponent<Animator>();
        foreach(Transform child in transform)
        {
            if(child.gameObject.name == "Easy" || child.gameObject.name == "Medium" || child.gameObject.name == "Hard")
                child.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction =  cam.ScreenToWorldPoint(Input.mousePosition) - cam.transform.position;
        hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), direction, direction.magnitude);
        
        buttons.Clear();

        foreach(Transform child in transform)
        {
            if(hit.transform == child)
            {
                buttons.Add(child.gameObject, Control.Selected);
            }
            else
                buttons.Add(child.gameObject, Control.NotSelected);
        }

        foreach(var button in buttons)
        {
            if(button.Value == Control.Selected)
                button.Key.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/UI/" + button.Key.name + " Button (Selected)") as Sprite;
            else
                button.Key.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/UI/" + button.Key.name + " Button") as Sprite;
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Slide"))
                foreach(GameObject button in buttons.Keys)
                    button.SetActive(false);
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("End"))
        {
            animator.enabled = false;
            foreach(GameObject button in buttons.Keys)
            {
                if(button.name == "Easy" || button.name == "Medium" || button.name == "Hard")
                    button.SetActive(true);
                else
                    button.SetActive(false);
            }   
        }
        if(Input.GetMouseButtonDown(0))
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                foreach(var button in buttons)
                    if(button.Value == Control.Selected)
                    {
                        if(button.Key.name.Equals("X - Axis"))
                            SceneParameters.control = ShieldController.Control.X_Axis;
                        if(button.Key.name.Equals("Wheel"))
                            SceneParameters.control = ShieldController.Control.MouseWheel;
                        if(button.Key.name.Equals("Arrows"))
                            SceneParameters.control = ShieldController.Control.Arrows;
                        animator.Play("Slide", 0);
                    }
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("End"))
                foreach(var button in buttons)
                    if(button.Value == Control.Selected)
                    {
                        if(button.Key.name.Equals("Easy"))
                            SceneParameters.ShieldDegrees = 45;
                        if(button.Key.name.Equals("Medium"))
                            SceneParameters.ShieldDegrees = 25;
                        if(button.Key.name.Equals("Hard"))
                            SceneParameters.ShieldDegrees = 15;
                        StartCoroutine(Wait());
                    }
            //SceneManager.UnloadSceneAsync("Assets/scenes/Menu.unity");
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("Assets/scenes/Static Scene.unity");
    }

}
