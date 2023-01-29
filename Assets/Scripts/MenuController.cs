using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public enum Control{Selected, NotSelected};

    public enum LoadedScene{Static, Dynamic, Platform};

    private LoadedScene scene;
    // Start is called before the first frame update
    public LayerMask mask;
    RaycastHit2D hit;

    public Camera cam;

    private Animator animator;

    public Dictionary<GameObject, Control> buttons = new Dictionary<GameObject, Control>();
    
    private GameObject selectedButton;

    public GameObject sceneButton;


    void Start()
    {
        scene = LoadedScene.Static;
        animator = GetComponent<Animator>();
        foreach(Transform child in transform)
        {
            if(child.gameObject.name == "Easy" || child.gameObject.name == "Medium" || child.gameObject.name == "Hard" || child.name == "Back")
                child.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction =  cam.ScreenToWorldPoint(Input.mousePosition) - cam.transform.position;
        hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), direction, direction.magnitude);
        
        buttons.Clear();
        switch(scene) 
        {
            case LoadedScene.Static:
                sceneButton.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/UI/Static Button") as Sprite;
                break;
            case LoadedScene.Dynamic:
                sceneButton.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/UI/Dynamic Button") as Sprite;
                break;
            case LoadedScene.Platform:
                sceneButton.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/UI/Platform Button") as Sprite;
                break;
        }       
        
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
                if(button.name == "Easy" || button.name == "Medium" || button.name == "Hard" || button.name == "Back")
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
                        if(button.Key.name.Equals("Demo"))
                        {
                            SceneParameters.isDemo = true;
                            switch(scene)
                            {
                                case LoadedScene.Static:
                                    SceneManager.LoadScene("Assets/scenes/Static Scene.unity");
                                    break;
                                case LoadedScene.Dynamic:
                                    SceneManager.LoadScene("Assets/scenes/Dynamic Scene.unity");
                                    break;
                                case LoadedScene.Platform:
                                    SceneManager.LoadScene("Assets/scenes/Platform Scene.unity");
                                    break;
                            }
                        }
                        if(button.Key.name.Equals("Right"))
                        {
                            if(scene == LoadedScene.Platform)
                                scene = LoadedScene.Static;
                            else
                                scene++;
                            return;
                        }
                        if(button.Key.name.Equals("Left"))
                        {
                            if(scene == LoadedScene.Static)
                                scene = LoadedScene.Platform;
                            else
                                scene--;
                            return;
                        }
                        if(button.Key.name.Equals("Exit"))
                        {
                            Application.Quit();
                            return;
                        }
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
                        if(button.Key.name.Equals("Back"))
                        {
                            animator.enabled = true;
                            foreach(GameObject b in buttons.Keys)
                            {
                                if(b.name == "Easy" || b.name == "Medium" || b.name == "Hard" || b.name == "Back")
                                    b.SetActive(false);
                                else
                                    b.SetActive(true);
                            }
                            
                            animator.Play("Idle", 0);
                            return;
                        }
                        StartCoroutine(Wait());
                    }
            //SceneManager.UnloadSceneAsync("Assets/scenes/Menu.unity");
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);

        SceneParameters.scene = scene;

        switch(scene) 
        {
            case LoadedScene.Static:
                SceneManager.LoadScene("Assets/scenes/Static Scene.unity");
                break;
            case LoadedScene.Dynamic:
                SceneManager.LoadScene("Assets/scenes/Dynamic Scene.unity");
                break;
            case LoadedScene.Platform:
                SceneManager.LoadScene("Assets/scenes/Platform Scene.unity");
                break;
        }
    }

}
