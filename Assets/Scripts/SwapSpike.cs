using UnityEngine;

public class SwapSpike : MonoBehaviour
{
    public Sprite Active;
    public Sprite Inactive;

    public string color;
    string type;

    GameObject GameManager;

    PolygonCollider2D pc;
    SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake() {
    }

    void Start() {
        GameManager = GameObject.FindGameObjectWithTag("Manager");
        var managerScript = GameManager.gameObject.GetComponent<GameManager>();
        pc = transform.gameObject.GetComponent<PolygonCollider2D>();
        sr = transform.gameObject.GetComponent<SpriteRenderer>();
        
        if (color == "Pink") {
            type = "Auto"; pc.enabled = true; sr.sprite = Active;
        } else if (color == "Berry") {
            type = "Auto"; pc.enabled = false; sr.sprite = Inactive;
        } else if (color == "Blue") {
            type = "Player"; pc.enabled = true; sr.sprite = Active;
        } else if (color == "Orange") {
            type = "Player"; pc.enabled = false; sr.sprite = Inactive;
        }
    }

    // Update is called once per frame
    void Update() {
        if (GameManager == null) {
            GameManager = GameObject.FindGameObjectWithTag("Manager");
            var managerScript = GameManager.gameObject.GetComponent<GameManager>();
        }
    }

    void FixedUpdate() {
        if (type == "Auto") {
            if (GameManager.gameObject.GetComponent<GameManager>().autoSpike == color) {
                if (pc.enabled == false) {pc.enabled = true; sr.sprite = Active;}
            } else if (pc.enabled == true) {
                pc.enabled = false; sr.sprite = Inactive;
            }
        } else if (type == "Player") {
            if (GameManager.gameObject.GetComponent<GameManager>().playSpike == color) {
            if (pc.enabled == false) {pc.enabled = true; sr.sprite = Active;}
            } else if (pc.enabled == true) {
                pc.enabled = false; sr.sprite = Inactive;
            }
        }
    }
}
