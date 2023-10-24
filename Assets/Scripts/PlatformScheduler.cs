using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlatformScheduler : MonoBehaviour
{


    public GameObject _countdown;
    private List<Scheduler.CSV> SavedData;
    public GameObject _player;

    private  List<GameObject> turrets;

    public GameObject LeftClick_Tutorial;

    public GameObject _endMenu;
    public GameObject X_Axis_Tutorial;

    public GameObject Wheel_Tutorial;

    private Animator countdown;

    public GameObject lightning;

    public GameObject _platform;

    private GameObject platform;

    public GameObject Spark;
    //public List<GameObject> enemies;
    public GameObject EndGoal;
    private GameObject player;
    private List<GameObject> lightnings;

    private int playerHealth;

    public List<float> distanceAngle;
    // Start is called before the first frame update
    public List<float> measuredTime;

    public string currentTime;
    public float countdownStart;
    private bool isEnd;
    private GameObject endMenu;

    private bool isTurretActive;
    void Awake()
    {
        isTurretActive = false;
        SavedData = new List<Scheduler.CSV>();
        countdownStart = 0.0f;
        isEnd = false;
        countdown = Instantiate(_countdown, new Vector3(0.0f, 1.5f, 0.0f), Quaternion.identity).GetComponent<Animator>();
        player = Instantiate(_player, Vector3.zero, Quaternion.identity);
        player.GetComponent<PlayerController>().endGoal = EndGoal;
        player.GetComponentInChildren<DrawCircle>().degrees = SceneParameters.ShieldDegrees;
        player.GetComponentInChildren<ShieldController>().control = SceneParameters.control;
        playerHealth = 3;
        
        if(SceneParameters.control == ShieldController.Control.X_Axis)
        {
            Destroy(Instantiate(X_Axis_Tutorial, new Vector3(-3.0f, -4.5f, 0.0f), Quaternion.identity), 3.0f);
            Destroy(Instantiate(LeftClick_Tutorial, new Vector3(2.0f, -4.5f, 0.0f), Quaternion.identity), 3.0f);
        }
        else if(SceneParameters.control == ShieldController.Control.MouseWheel)
        {
            Destroy(Instantiate(Wheel_Tutorial, new Vector3(-3.0f, -4.5f, 0.0f), Quaternion.identity), 3.0f);
            Destroy(Instantiate(LeftClick_Tutorial, new Vector3(2.0f, -4.5f, 0.0f), Quaternion.identity), 3.0f);
        }
    }
    void Start()
    {
        turrets = new List<GameObject>();
        GameObject[] objects = (GameObject[]) UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
        foreach(GameObject turret in objects)
        {
            if(turret.name.StartsWith("Turret"))
            {
                turrets.Add(turret);
                turret.SetActive(false);
            }   
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        lightnings = new List<GameObject>();
    }
    void Update()
    {
        if(player != null && playerHealth > player.GetComponent<PlayerController>().currentHealth)
        {
            playerHealth = player.GetComponent<PlayerController>().currentHealth;
            isTurretActive = false;
        }

        foreach(GameObject turret in turrets)
        {
            if(turret != null && !isTurretActive && (turret.transform.position - player.transform.position).magnitude < 6.0f)
            {
                countdownStart = Time.time;

                turret.SetActive(true);

                isTurretActive = true;

                Vector2 shieldAngle = player.GetComponentInChildren<PolygonCollider2D>().bounds.center - player.transform.position;

                Vector2 enemyAngle = turret.transform.position - player.transform.position;

                Vector2 difference = enemyAngle - shieldAngle;

                distanceAngle.Add(Vector2.Angle(shieldAngle, difference));

                turret.GetComponent<Aim>().player = player;
            }
        }
        onClick();
    }
    

    private void OnGUI() {
        if(!isEnd)
            currentTime = GUI.TextArea(new Rect(40, 20, 150, 50), currentTime, 50);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(distanceAngle.Count != 0 && isTurretActive)
            currentTime = (Time.time - countdownStart).ToString() + " s\nDistance angle: " + distanceAngle[distanceAngle.Count-1];

        if(endMenu != null)
        {
            FadeLightning();
            return;
        }   

        if((Time.time - countdownStart) >= 2.0f)
        {
            countdownStart = 0.0f;
            isTurretActive = false;
        }

        if(countdown.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) return;

        if(player.GetComponent<PlayerController>().IsAtEnd())
        {
            Camera.main.transform.position = new Vector3(-6.0f, -1.0f, -10.0f);
            foreach(GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
            {
                if(obj.name != "Main Camera")
                    obj.SetActive(false);
            }
            SaveToCSV();
            //outputString += health == 1 ? "1 life remains" : health.ToString() + " lives remain";
            //SaveToCSV();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            endMenu = Instantiate(_endMenu, Vector3.zero, Quaternion.identity);
            endMenu.GetComponent<ShowScore>().RenderText(SavedData);
            if(SceneParameters.isDemo)
            {
                if(SceneParameters.ShieldDegrees == 45)
                    SceneParameters.ShieldDegrees = 25;
                else
                    SceneParameters.ShieldDegrees = 15;
            }
            isEnd = true;
        }
        else if(player == null || player.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "Death")
        {
            if(player == null)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene("Menu");
            }
        }
        Camera.main.transform.position += new Vector3((player.transform.position.x - Camera.main.transform.position.x)/ 15, (player.transform.position.y + 1.0f - Camera.main.transform.position.y)/ 20, 0.0f);
        FadeLightning();
    }
    void SaveToCSV()
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')) + "\\data\\";
                if(SceneParameters.control == ShieldController.Control.X_Axis)
                    path += "X-Axis\\";
                else if(SceneParameters.control == ShieldController.Control.MouseWheel)
                    path += "Wheel\\";
                else
                    path += "Arrows\\";

                if(SceneParameters.scene == MenuController.LoadedScene.Static)
                    path += "Static\\result.csv";
                else if(SceneParameters.scene == MenuController.LoadedScene.Dynamic)
                    path += "Dynamic\\result.csv";
                else
                    path += "Platform\\result.csv";


            if (!File.Exists(path))
            {
                var firstLine = "Angle; Time; Shield_Angle" + Environment.NewLine;
                File.WriteAllText(path, firstLine);
            }

            //writer.Write("Angle, Time, Shield Angle");
            //writer.Write(System.Environment.NewLine);

            foreach(Scheduler.CSV csv in SavedData)
            {
                var line = String.Format("{0}; {1}; {2}", csv.angle.ToString("0.00"), csv.time.ToString("0.00000"), csv.shieldAngle) + Environment.NewLine;
                File.AppendAllText(path, line);
            }
        }
    void FadeLightning()
    {
        for(int i = 0; i < lightnings.Count; i++)
            if(lightnings[i].GetComponent<LightningBolt>().isFaded())
            {
                Destroy(lightnings[i]);
                lightnings.RemoveAt(i--);
            }
    }

    void onClick()
    {
        if((Input.GetMouseButtonDown(0) && (SceneParameters.control == ShieldController.Control.X_Axis || SceneParameters.control == ShieldController.Control.MouseWheel)) ||
        (Input.GetKeyDown("space") && SceneParameters.control == ShieldController.Control.Arrows))
        for(int i = 0; i < turrets.Count; i++)
        {
            if(turrets[i] != null && (turrets[i].GetComponent<Aim>().onBlock && turrets[i].GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length != 0 && turrets[i].GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name != "Firing"))
                {
                    isTurretActive = false;
                    SavedData.Add(new Scheduler.CSV
                    {
                        time = Time.time - countdownStart,
                        angle = distanceAngle[distanceAngle.Count - 1],
                        shieldAngle = player.GetComponentInChildren<DrawCircle>().degrees
                    });
                    countdownStart = 0.0f;
                    player.GetComponentInChildren<DrawCircle>().Reflect();
                    lightnings.Add(Instantiate(lightning, Vector3.zero, Quaternion.identity));
                    lightnings.Add(Instantiate(lightning, Vector3.zero, Quaternion.identity));
                    lightnings[lightnings.Count - 1].GetComponent<LightningBolt>().SetRenderer(player.transform.position, turrets[i].GetComponent<Aim>().GetHitPoint(), true);
                    lightnings[lightnings.Count - 2].GetComponent<LightningBolt>().SetRenderer(player.transform.position, turrets[i].GetComponent<Aim>().GetHitPoint(), false);
                    Destroy(Instantiate<GameObject>(Spark, turrets[i].GetComponent<Aim>().GetHitPoint(), turrets[i].transform.rotation), 0.4f);
                    turrets[i].GetComponent<Aim>().Shoot();
                    Destroy(turrets[i], 2.0f);
                    turrets.RemoveAt(i);
                }
        }
    }
}
