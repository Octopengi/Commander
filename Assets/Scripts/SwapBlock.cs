using UnityEngine;

public class SwapBlock : MonoBehaviour
{
    public Sprite Active;
    public Sprite Inactive;
    public GameObject antiClip;

    public string color;
    string type;

    GameObject GameManager;

    BoxCollider2D bc;
    SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        GameManager = GameObject.FindGameObjectWithTag("Manager");
        var managerScript = GameManager.gameObject.GetComponent<GameManager>();
        bc = transform.gameObject.GetComponent<BoxCollider2D>();
        sr = transform.gameObject.GetComponent<SpriteRenderer>();
        if (color == "Pink") {
            type = "Auto"; bc.enabled = true; sr.sprite = Active; antiClip.SetActive(true);
        } else if (color == "Berry") {
            type = "Auto"; bc.enabled = false; sr.sprite = Inactive; antiClip.SetActive(false);
        } else if (color == "Blue") {
            type = "Player"; bc.enabled = true; sr.sprite = Active; antiClip.SetActive(true);
        } else if (color == "Orange") {
            type = "Player"; bc.enabled = false; sr.sprite = Inactive; antiClip.SetActive(false);
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
                if (!bc.enabled) {bc.enabled = true; antiClip.SetActive(true); sr.sprite = Active;}
            } else if (bc.enabled) {
                bc.enabled = false; antiClip.SetActive(false); sr.sprite = Inactive;
            }
        } else if (type == "Player") {
            if (GameManager.gameObject.GetComponent<GameManager>().playSpike == color) {
                if (!bc.enabled) {bc.enabled = true; antiClip.SetActive(true); sr.sprite = Active;} 
            } else if (bc.enabled) {
                bc.enabled = false; antiClip.SetActive(false); sr.sprite = Inactive;
            }
        }
        
    }
}
