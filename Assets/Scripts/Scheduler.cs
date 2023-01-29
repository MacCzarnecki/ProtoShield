using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Scheduler : MonoBehaviour
{

    [Serializable]
    public struct Json 
    {
        public float time;
        public float angle;
        public int shieldAngle;
    }

    public GameObject _countdown;
    private List<Json> SavedData;
    public GameObject _player;

    public GameObject LeftClick_Tutorial;

    public GameObject _endMenu;
    public GameObject X_Axis_Tutorial;

    public GameObject Wheel_Tutorial;

    private Animator countdown;
    public GameObject turret;

    public GameObject lightning;

    public GameObject _platform;

    private GameObject platform;

    public GameObject Spark;
    //public List<GameObject> enemies;

    private GameObject enemy;

    private GameObject player;
    private List<GameObject> lightnings;

    public List<float> distanceAngle;
    // Start is called before the first frame update
    public List<float> measuredTime;

    public string currentTime;
    public float countdownStart;
    private bool isEnd;
    private GameObject endMenu;
    void Awake()
    {
        SavedData = new List<Json>();
        countdownStart = 0.0f;
        isEnd = false;
        countdown = Instantiate(_countdown, new Vector3(0.0f, 1.5f, 0.0f), Quaternion.identity).GetComponent<Animator>();
        player = Instantiate(_player, Vector3.zero, Quaternion.identity);
        player.GetComponentInChildren<DrawCircle>().degrees = SceneParameters.ShieldDegrees;
        player.GetComponentInChildren<ShieldController>().control = SceneParameters.control;
        platform = Instantiate(_platform, new Vector3(0.0f, -1.0f, 0.0f), Quaternion.identity);
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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        lightnings = new List<GameObject>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && (SceneParameters.control == ShieldController.Control.X_Axis || SceneParameters.control == ShieldController.Control.MouseWheel))
            onClick();
        if(Input.GetKeyDown("space") && SceneParameters.control == ShieldController.Control.Arrows)
            onClick();
    }

    private void OnGUI() {
        if(!isEnd)
            currentTime = GUI.TextArea(new Rect(40, 20, 150, 50), currentTime, 50);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(endMenu != null)
        {
            FadeLightning();
            return;
        }
        if(countdown.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return;
        if(SavedData.Count >= 15)
        {
            /*string outputString = "{\n\"data\": [";
            foreach(Json json in SavedData)
            {
                outputString += "\n" + JsonUtility.ToJson(json) + ",";
            }
            outputString = outputString.TrimEnd(',');
            outputString += "\n]\n}";
            int health = player.GetComponent<PlayerController>().currentHealth;
            //outputString += health == 1 ? "1 life remains" : health.ToString() + " lives remain";
            string path = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')) + "\\data\\";
            if(SceneParameters.control == ShieldController.Control.X_Axis)
                path += "X-Axis\\";
            else if(SceneParameters.control == ShieldController.Control.MouseWheel)
                path += "Wheel\\";
            else
                path += "Arrows\\";

            if(SceneParameters.scene == MenuController.LoadedScene.Static)
                path += "Static\\";
            else if(SceneParameters.scene == MenuController.LoadedScene.Dynamic)
                path += "Dynamic\\";
            else
                path += "Platform\\";

            var info = new DirectoryInfo(path);
            
            File.WriteAllText(path + info.GetFiles().Length.ToString() + ".json", outputString);*/
            SaveToCSV();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            endMenu = Instantiate(_endMenu, Vector3.zero, Quaternion.identity);
            endMenu.GetComponent<ShowScore>().RenderText(SavedData);
            Destroy(player);
            Destroy(platform);
            Destroy(enemy);
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
            if(enemy == null && player == null)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene("Menu");
            }
        }
        else
        {
            if(enemy == null)
                makeNewTurret(); 
            if(enemy.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length != 0 && enemy.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name != "Firing")
            {
                currentTime = (Time.time - countdownStart).ToString() + " s\nDistance angle: " + distanceAngle[distanceAngle.Count-1];
            }
        }

        FadeLightning();
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
            var firstLine = "Angle, Time, Shield_Angle" + Environment.NewLine;
            File.WriteAllText(path, firstLine);
        }

        //writer.Write("Angle, Time, Shield Angle");
        //writer.Write(System.Environment.NewLine);

        foreach(Json json in SavedData)
        {
            var line = String.Format("{0}, {1}, {2}", json.angle.ToString("0.00").Replace(',','.'), json.time.ToString("0.00000").Replace(',','.'), json.shieldAngle) + Environment.NewLine;
            File.AppendAllText(path, line);
        }
    }
    void makeNewTurret()
    {
        countdownStart = Time.time;

        float radius = 5.0f;

        System.Random rnd = new System.Random();
        double radian = rnd.NextDouble();

        float xScaled = Mathf.Cos((float)radian * 2 * Mathf.PI);
        float yScaled = Mathf.Sin((float)radian * 2 * Mathf.PI);

        float x = player.transform.position.x + xScaled * radius;
        float y = player.transform.position.y + yScaled * radius;
        
        enemy = Instantiate(turret, new Vector3(x, y, 0), Quaternion.identity);
        enemy.GetComponent<Aim>().player = player;
        //enemies.Add(Instantiate(turret, new Vector3(x, y, 0), Quaternion.identity));
        //enemies[enemies.Count - 1].GetComponent<Aim>().player = player;

        Vector2 shieldAngle = player.GetComponentInChildren<PolygonCollider2D>().bounds.center - player.transform.position;

        Vector2 enemyAngle = enemy.transform.position - player.transform.position;

        Vector2 difference = enemyAngle - shieldAngle;

        distanceAngle.Add(Vector2.Angle(shieldAngle, difference));
        
    }

    void onClick()
    {

        if(enemy != null && enemy.GetComponent<Aim>().onBlock)
        {
            SavedData.Add(new Json
            {
                time = Time.time - countdownStart,
                angle = distanceAngle[distanceAngle.Count - 1],
                shieldAngle = player.GetComponentInChildren<DrawCircle>().degrees
            });
            player.GetComponentInChildren<DrawCircle>().Reflect();
            lightnings.Add(Instantiate(lightning, Vector3.zero, Quaternion.identity));
            lightnings.Add(Instantiate(lightning, Vector3.zero, Quaternion.identity));
            lightnings[lightnings.Count - 1].GetComponent<LightningBolt>().SetRenderer(player.transform.position, enemy.GetComponent<Aim>().GetHitPoint(), true);
            lightnings[lightnings.Count - 2].GetComponent<LightningBolt>().SetRenderer(player.transform.position, enemy.GetComponent<Aim>().GetHitPoint(), false);
            Destroy(Instantiate<GameObject>(Spark, enemy.GetComponent<Aim>().GetHitPoint(), enemy.transform.rotation), 0.4f);
            enemy.GetComponent<Aim>().Shoot();
            if(SavedData.Count < 15)
                makeNewTurret();
        }
    }
}
