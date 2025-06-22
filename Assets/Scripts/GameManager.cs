using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public RawImage deathScreen;
    public Image guageBase;
    public GameObject guageUI;
    public GameObject menuUI;
    public GameObject player;
    public GameObject playPrefab;
    public GameObject textPanel;
    public GameObject dcPanel;
    public TMP_Text panelText;
    public TMP_Text lvlNumText;
    public TMP_Text dcText;

    PlayerCont playCont;

    float autoSpikeCD = 1.5f;
    float playSpikeCD = 0.0f;
    int level = 0;
    int deaths = -1;
    Dictionary<int, int> finalChecks = new Dictionary<int, int>();

    public string autoSpike = "Pink";
    public string playSpike = "Blue";

    bool transition = false;
    string currScene;

    void Awake() {
        currScene = SceneManager.GetActiveScene().name;

        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        } else {
            Destroy(this.gameObject);
            return;
        }
        
        if (SceneManager.GetActiveScene().name.Contains("Level")) {
            var s = GameObject.Find("Start");
            Instantiate(playPrefab, s.transform.position, s.transform.rotation);
            
            player = GameObject.FindGameObjectWithTag("Player");
            playCont = player.gameObject.GetComponent<PlayerCont>();

            level = int.Parse(currScene[currScene.Length - 1].ToString());

            guageUI.SetActive(true); menuUI.SetActive(false);
            deaths = 0;
        } else {
            guageUI.SetActive(false); menuUI.SetActive(true);
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

    }

    // Update is called once per frame
    void Update()
    {
        currScene = SceneManager.GetActiveScene().name;
        if (currScene.Contains("Level") && !transition) { LevelUpdate(); }
        if (Input.GetKeyDown(KeyCode.O)) { LevelSelected(100); }
        if (Input.GetKey(KeyCode.L))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {LevelSelected(1);}
            if (Input.GetKeyDown(KeyCode.Alpha2)) {LevelSelected(2);}
            if (Input.GetKeyDown(KeyCode.Alpha3)) {LevelSelected(3);}
        }
    }

    void FixedUpdate() {
        autoSpikeCD -= Time.deltaTime;
        if (playSpikeCD > 0.0f) {
            playSpikeCD -= Time.deltaTime;
        } else {
            playSpikeCD = 0.0f;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode Single) {
        if (scene.name.Contains("Level")) {
            if (player == null) {
                var s = GameObject.Find("Start");
                Instantiate(playPrefab, s.transform.position, s.transform.rotation);
                
                player = GameObject.FindGameObjectWithTag("Player");
                playCont = player.gameObject.GetComponent<PlayerCont>();

                guageUI.SetActive(true);
                menuUI.SetActive(false);
                deaths = 0;
            }
        } else if (scene.name == "MainMenu") {
            guageUI.SetActive(false);
            menuUI.SetActive(true);
            if (deaths != -1) {
                dcPanel.SetActive(true);
                lvlNumText.text = "Level " + level.ToString() + " Complete!";
                dcText.text = "Death Count: " + deaths.ToString();
                deaths = -1; level = 0;
            }
        }
    }

    void LevelUpdate() {
        if (autoSpikeCD <= 0) {
        if (autoSpike == "Pink") {autoSpike = "Berry";}
        else if (autoSpike == "Berry") {autoSpike = "Pink";}
        autoSpikeCD = 1.5f;
        }
        if (Input.GetMouseButtonDown(1) && playSpikeCD <= 0) {
            if (playSpike == "Blue") {playSpike = "Orange";}
            else if (playSpike == "Orange") {playSpike = "Blue";}
            playSpikeCD = 1.25f;
        }

        //FUEL GUAGE UI
        guageBase.fillAmount = 0.75f * playCont.GetFuel() / 100;
        if (playCont.engDead) {
            guageBase.color = new Color(0, 0, 150);
        } else {
            guageBase.color = new Color(255, 255, 255);
        }
    }

    public void LevelSelected(int loadLevel) {
        if (!transition) {
            level = loadLevel; menuUI.SetActive(false); dcPanel.SetActive(false);
            SceneSwap("Level" + level.ToString());
        }
    }

    public bool IsTransition() {
        return transition;
    }

    public void DeathTransition() {
        StartCoroutine(RunDeathTransition());
        deaths += 1;
    }

    public bool CheckCheckpoint(int checkNum) {
        return checkNum == finalChecks[level];
    }

    public void HideText() {
        textPanel.SetActive(false);
        panelText.text = "";
    }

    public void ShowText(string text) {
        if (transition) {return;}
        else {
            panelText.text = text;
            textPanel.SetActive(true);
        }
    }

    public void SceneSwap(string scene) {
        StartCoroutine(RunSceneTransition(scene));
    }

    IEnumerator RunDeathTransition() {
        transition = true;
        float opacity = 0;
        float wait = 0.4f;
        Color deathColor = new Color(255,255,255,0);

        while (opacity < 1) {
            opacity += 5 * Time.deltaTime;
            deathColor.a = opacity; deathScreen.color = deathColor;
            yield return null;
        }

        player.GetComponent<PlayerCont>().playRespawn();

        Vector3 pos = player.GetComponent<PlayerCont>().GetRespawn();
        pos.z = -10;

        Camera.main.transform.position = pos;
        while (wait > 0) {
            wait -= Time.deltaTime;
            yield return null;
        }

        player.SetActive(true);

        while (opacity > 0) {
            opacity -= 5 * Time.deltaTime;
            deathColor.a = opacity; deathScreen.color = deathColor;
            yield return null;
        }
        deathColor.a = 0; deathScreen.color = deathColor;
        autoSpike = "Pink"; //playSpike = "Blue";
        autoSpikeCD = 1.5f; playSpikeCD = 0.0f;
        transition = false;
    }

    IEnumerator RunSceneTransition(string scene) {
        transition = true;
        float opacity = 0; //float wait = 0.4f;
        Color imageColor = new Color(255, 255, 255, 0);

        while (opacity < 1) {
            opacity += 5 * Time.deltaTime;
            imageColor.a = opacity; deathScreen.color = imageColor;
            yield return null;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        yield return null;
        
        while (opacity > 0) {
            opacity -= 5 * Time.deltaTime;
            imageColor.a = opacity; deathScreen.color = imageColor;
            yield return null;
        }

        imageColor.a = 0; deathScreen.color = imageColor;
        transition = false;
    }

}
