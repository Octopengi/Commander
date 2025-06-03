using UnityEngine;

public class LastCheck : MonoBehaviour {
    public static LastCheck instance;
    GameObject player;
    GameObject blackHole;
    GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        blackHole = GameObject.Find("Blackhole");
        General.PointToBH(this.gameObject);
        player = GameObject.FindGameObjectWithTag("Player");
        if (instance == null) {
            instance = this;
            gameManager = GameManager.instance;
        } else {
            Destroy(this.gameObject);
            return; 
        }
    }

    // Update is called once per frame
    void Update() {
        if (blackHole == null) {
            blackHole = GameObject.Find("Blackhole");
            General.PointToBH(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            gameManager.SceneSwap("MainMenu");
        }
    }
}
