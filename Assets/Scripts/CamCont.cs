using UnityEngine;

public class CamCont : MonoBehaviour
{
    Vector2 topL;
    Vector2 botR;
    public GameObject blackHole;
    GameObject player;
    Transform camMount;
    float lerpSPD = 5.0f;
    bool clampActive = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        camMount = player.GetComponent<PlayerCont>().camMount;
        var pos = transform.position;
        pos.x = player.transform.position.x; pos.y = player.transform.position.y;
        transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            camMount = player.GetComponent<PlayerCont>().camMount;
            var pos = transform.position;
            pos.x = player.transform.position.x; pos.y = player.transform.position.y;
            transform.position = pos;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            clampActive = !clampActive;
            Debug.Log(clampActive);
        }
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            var pos = transform.position;
            /*pos.x = player.transform.position.x;
            pos.y = player.transform.position.y;*/
            pos.x = camMount.transform.position.x; pos.y = camMount.transform.position.y;
            if (clampActive)
            {
                pos.x = Mathf.Clamp(pos.x, topL.x, botR.x);
                pos.y = Mathf.Clamp(pos.y, botR.y, topL.y);
            }

            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * lerpSPD);
        }

        if (blackHole != null)
        {
            /*var dir = blackHole.transform.position - transform.position;
            dir.Normalize();
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);*/
            transform.rotation = player.transform.rotation;
        }
    }

    public void SetClamp(Vector2 newTopL, Vector2 newBotR)
    {
        topL = newTopL;
        botR = newBotR;
    }
}
