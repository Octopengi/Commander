using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkNum;
    //public float respawnX;
    //public float respawnY;
    public Vector2 respawn;
    public GameObject spawn;
    GameObject blackHole;
    GameObject player;

    SpriteRenderer sr;
    public Sprite active;
    public Sprite inactive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        //respawnX = spawn.transform.position.x;
        //respawnY = spawn.transform.position.y;
        respawn = spawn.transform.position;

        blackHole = GameObject.Find("Blackhole");
        General.PointToBH(this.gameObject);
        player = GameObject.FindGameObjectWithTag("Player");
        

        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        if (blackHole == null) {
            blackHole = GameObject.Find("Blackhole");
            General.PointToBH(this.gameObject);
        }
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        } else {
            if (player.GetComponent<PlayerCont>().checkNum == checkNum) {
                sr.sprite = active;
            } else {
                sr.sprite = inactive;
            }
        }
    }
}
