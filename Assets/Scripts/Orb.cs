using System.Net.Http.Headers;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public string type;
    public Sprite active;
    public Sprite inactive;
    public float cd;
    public float currCD = 0.0f;

    SpriteRenderer sr;
    CircleCollider2D cc2d;
    GameObject blackHole;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        blackHole = GameObject.Find("Blackhole");

        General.PointToBH(this.gameObject);

        sr = gameObject.GetComponent<SpriteRenderer>();
        cc2d = gameObject.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update() {
        if (blackHole == null) {
            blackHole = GameObject.Find("Blackhole");
            General.PointToBH(this.gameObject);
        }

        if (currCD <= 0) {
            sr.sprite = active;
            cc2d.enabled = true;
        }
    }

    void FixedUpdate() {
        if (currCD > 0) {
            currCD -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player") && currCD <= 0) {
            sr.sprite = inactive; currCD = cd; cc2d.enabled = false;
        }
    }
}
