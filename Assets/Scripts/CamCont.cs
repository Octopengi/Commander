using UnityEngine;

public class CamCont : MonoBehaviour
{
    public Vector2 topL;
    public Vector2 botR;
    public GameObject blackHole;
    GameObject player;
    float lerpSPD = 5.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        var pos = transform.position;
        pos.x = player.transform.position.x; pos.y = player.transform.position.y;
        transform.position = pos;
    }

    // Update is called once per frame
    void Update() {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
            var pos = transform.position;
            pos.x = player.transform.position.x; pos.y = player.transform.position.y;
            transform.position = pos;
        }
    }

    void FixedUpdate() {
        if (player != null) {
            var pos = transform.position;
            pos.x = player.transform.position.x;
            pos.y = player.transform.position.y;

            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * lerpSPD);
        }

        if (blackHole != null) {
            var dir = blackHole.transform.position - transform.position;
            dir.Normalize();
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
        }
    }
}
