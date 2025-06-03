using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FuelGuage : MonoBehaviour
{
    GameObject player;
    public Image guageBase;
    PlayerCont playCont;
    string currScene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (SceneManager.GetActiveScene().name.Contains("Level")) {
            player = GameObject.FindGameObjectWithTag("Player");
            playCont = player.gameObject.GetComponent<PlayerCont>();
        }
    }

    // Update is called once per frame
    void Update() {
        if (player == null && SceneManager.GetActiveScene().name.Contains("Level")) {
            player = GameObject.FindGameObjectWithTag("Player");
        } else {
            guageBase.fillAmount = 0.75f * playCont.GetFuel() / 100;
            if (playCont.engDead) {
                guageBase.color = new Color(0, 0, 150);
            } else {
                guageBase.color = new Color(255, 255, 255);
            }
        }
    }
}
