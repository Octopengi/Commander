using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float spd = 1.0f;
    public string follow;
    GameObject player;
    float x; float y;

    void Awake() {
        if (follow == "player") player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        if (follow == "player") {
            if (player == null) {
                player = GameObject.FindGameObjectWithTag("Player");
                transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
            } else {
                //transform.rotation = player.transform.rotation;
                General.PointToBH(this.gameObject, 0.1f, true);
            }
            x = Camera.main.transform.position.x;
            y = Camera.main.transform.position.y;
        } else if (follow == "mouse") {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            x = mousePos.x; y = mousePos.y;
        }

        x *= spd; y *= spd / 2;
        var pos = transform.position;
        pos.x = x; pos.y = y;
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 0.25f);
        transform.position = pos;
    }

}
