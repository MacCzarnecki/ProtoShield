using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
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

    public GameObject X_Axis_Tutorial;

    private Animator countdown;
    public GameObject turret;

    public GameObject lightning;

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

    void Awake()
    {
        SavedData = new List<Json>();
        countdownStart = 0.0f;
        
        countdown = Instantiate(_countdown, new Vector3(0.0f, 1.5f, 0.0f), Quaternion.identity).GetComponent<Animator>();
        player = Instantiate(_player, Vector3.zero, Quaternion.identity);
        player.GetComponentInChildren<DrawCircle>().degrees = SceneParameters.ShieldDegrees;
        player.GetComponentInChildren<ShieldController>().control = SceneParameters.control;
        if(SceneParameters.control == ShieldController.Control.X_Axis)
            Destroy(Instantiate(X_Axis_Tutorial, new Vector3(0.0f, -2.0f, 0.0f), Quaternion.identity), 3.0f);
    }
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        lightnings = new List<GameObject>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
            onClick();
    }

    private void OnGUI() {
        currentTime = GUI.TextArea(new Rect(40, 20, 150, 50), currentTime, 50);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(countdown.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return;
        if(player == null || player.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "Death")
        {
            if(enemy == null && player == null)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene("Menu");
            }
        }
        else if(SavedData.Count < 15)
        {
            // if(enemies.TrueForAll(delegate(GameObject enemy)
            // {
            //     if(enemy == null) return true;
            //     else return false;
            // }))
            if(enemy == null || (enemy.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length != 0 && enemy.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "Teleporting"))
               makeNewTurret(); 
            if(enemy.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length != 0 && enemy.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "Loading")
                if(countdownStart == 0.0f)
                    countdownStart = Time.time;
            if(enemy.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length != 0 && enemy.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name != "Firing")
            {
                if(countdownStart != 0.0f)
                    currentTime = (Time.time - countdownStart).ToString() + " s\nDistance angle: " + distanceAngle[distanceAngle.Count-1];
            }
            /*if(enemies.Count < 1)
                makeNewTurret();
            if(enemies[0] == null)
            {
                Destroy(enemies[0]);
                enemies.RemoveAt(0);
                //measuredTime.Add(float.PositiveInfinity);
                distanceAngle.RemoveAt(distanceAngle.Count - 1);
            }
            for(int i = 0; i < enemies.Count; i++)
            {
                if(enemies[i] != null)
                    currentTime = enemies[i].GetComponent<Aim>().returnTime().ToString() + " s\nDistance angle: " + distanceAngle[i];
            }*/
        }
        else
        {
            string outputString = "";
            foreach(Json json in SavedData)
            {
                outputString += JsonUtility.ToJson(json) + "\n";
            }
            int health = player.GetComponent<PlayerController>().currentHealth;
            //outputString += health == 1 ? "1 life remains" : health.ToString() + " lives remain";
            File.WriteAllText("result.txt", outputString);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Menu");
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

    void makeNewTurret()
    {
        countdownStart = 0.0f;

        float radius = 3f;

        float radian = UnityEngine.Random.Range(0.0f, 1.0f);

        float xScaled = Mathf.Cos(radian * 2 * Mathf.PI);
        float yScaled = Mathf.Sin(radian * 2 * Mathf.PI);

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

        // countdownStart = 0.0f;
        // measuredTime.Add(float.PositiveInfinity);
        // SavedData.Add(new Json
        // {
        //     time = float.PositiveInfinity,
        //     angle = distanceAngle[distanceAngle.Count - 1],
        //     shieldAngle = player.GetComponentInChildren<DrawCircle>().degrees
        // });
        // Instantiate<GameObject>(Spark, player.transform.position, enemy.transform.rotation);
        // player.GetComponent<PlayerController>().TakeDamage();
        // enemy.GetComponent<Aim>().Shoot();
        if(enemy != null && enemy.GetComponent<Aim>().onBlock)
        {
            countdownStart = 0.0f;
            SavedData.Add(new Json
            {
                time = enemy.GetComponent<Aim>().returnTime(),
                angle = distanceAngle[distanceAngle.Count - 1],
                shieldAngle = player.GetComponentInChildren<DrawCircle>().degrees
            });
            lightnings.Add(Instantiate(lightning, Vector3.zero, Quaternion.identity));
            lightnings.Add(Instantiate(lightning, Vector3.zero, Quaternion.identity));
            lightnings[lightnings.Count - 1].GetComponent<LightningBolt>().SetRenderer(player.transform.position, enemy.GetComponent<Aim>().GetHitPoint(), true);
            lightnings[lightnings.Count - 2].GetComponent<LightningBolt>().SetRenderer(player.transform.position, enemy.GetComponent<Aim>().GetHitPoint(), false);
            Instantiate<GameObject>(Spark, enemy.GetComponent<Aim>().GetHitPoint(), enemy.transform.rotation);
            enemy.GetComponent<Aim>().Shoot();
        }
    }
}
