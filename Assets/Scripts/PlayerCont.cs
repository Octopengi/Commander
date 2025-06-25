using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCont : MonoBehaviour
{
    GameObject blackHole;

    Rigidbody2D rb;
    CapsuleCollider2D cc;
    SpriteRenderer sr;
    Transform feet;
    Transform point;
    Vector2 feetBox = new Vector2(0.25f, 0.15f);
    LayerMask groundMask;
    GameManager gameManager;

    public float spdMLT = 10;
    public float jumpMLT = 9;
    public float hvForce = 1.8f;
    public float dashForce = 11.4f;
    public float fuelRate = 0.13f;
    public float dashFuel = 13.5f;
    public int checkNum = 0;

    public Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();
    public List<Sprite> spriteList = new List<Sprite>();

    bool grounded;
    bool jump;
    bool hover;
    bool dash = false;
    bool bhImmune = false;
    public bool engDead = false;
    bool fastFall = false;
    float fuel = 100.0f;
    float move;
    float dashTime = 0.0f;
    float initLinDamp;
    string direct = "R";

    Vector2 respawn = new Vector2(0.0f, 0.0f);
    Vector2 ccNormSz = new Vector2(0.35f, 1.0f);
    Vector2 ccDashSz = new Vector2(0.28f, 0.28f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        initLinDamp = rb.linearDamping;
        sr = GetComponent<SpriteRenderer>();
        cc = GetComponent<CapsuleCollider2D>();
        feet = transform.Find("Feet");
        point = transform.Find("Point");
        groundMask = LayerMask.GetMask("Ground");
        respawn = transform.position;
        //blackHole = GameObject.Find("Blackhole");
        for (int i = 0; i < spriteList.Count; ++i) {
            spriteDict[spriteList[i].name] = spriteList[i];
        }
        sr.sprite = spriteDict["slosh_R_0"];
        gameManager = GameManager.instance;
        //Debug.Log("Can hover for ~" + (100 / fuelRate) + "s");
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.P))
        {
            bhImmune = true;
            gameManager.SceneSwap("MainMenu");
        }
        if (blackHole == null) blackHole = GameObject.Find("Blackhole");
        grounded = Physics2D.OverlapBox(feet.position, feetBox, 0, groundMask);
        if (dashTime > 0) grounded = false;
        move = Input.GetAxis("Horizontal");

        if (Input.GetButton("Jump") && !engDead) {
            hover = true;
            fuel -= fuelRate * Time.deltaTime;
            //fuel -= 0.13f;
            //fuel -= fuelRate;
            if (fuel <= 0) {
                engDead = true;
            }
        } else hover = false;

        if (Input.GetMouseButtonDown(0) && fuel >= dashFuel && !engDead && dashTime <= 0.0f) {
            cc.size = ccDashSz; rb.linearDamping = 0.0f;
            dash = true; fuel -= dashFuel;
            if (fuel <= 0) {
                engDead = true;
            }
        }

        if (Input.GetKey(KeyCode.S) && !grounded && !hover) {
            fastFall = true;
        } else fastFall = false;

        if (engDead && fuel >= 40.0f) {
            engDead = false;
        }

        if (grounded && fuel < 100) {fuel += fuelRate * 1.7f * Time.deltaTime;}
        if (fuel > 100) {fuel = 100.0f;}

        ChangeSprite();
    }

    void FixedUpdate() {
        var mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); mPos.z = 0;
        var mDir = mPos - transform.position; mDir.Normalize();
        if (dashTime > 0)
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
            {
                cc.size = ccNormSz; rb.linearDamping = initLinDamp;
                rb.linearVelocityX /= 1.26f; rb.linearVelocityY /= 1.26f;
            }
            else if (dashTime < 0.08f && rb.linearDamping != initLinDamp)
            {
                rb.linearVelocityX = Mathf.Lerp(rb.linearVelocityX, rb.linearVelocityX / 3f, Time.deltaTime * 7.8f);
                rb.linearVelocityY = Mathf.Lerp(rb.linearVelocityY, rb.linearVelocityY / 3f, Time.deltaTime * 7.8f);
                rb.linearDamping = Mathf.Lerp(rb.linearDamping, initLinDamp, Time.deltaTime * 5.5f);
            }
        }
        /*if (blackHole != null) {
            var dir = blackHole.transform.position - transform.position;
            dir.Normalize();
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
        }*/

        General.PointToBH(this.gameObject);

        if (point != null) {
            float mAngle = Mathf.Atan2(mDir.y, mDir.x) * Mathf.Rad2Deg;
            point.transform.rotation = Quaternion.AngleAxis(mAngle + 90, Vector3.forward);
        }

        if (move != 0) {
            rb.AddForce(transform.right * move * spdMLT);
            if (move > 0) {direct = "R";}
            else direct = "L";
        }

        if (jump) {
            jump = false;
            rb.AddForce(transform.up * jumpMLT, ForceMode2D.Impulse);
        }

        if (hover) {
            rb.AddForce(transform.up * hvForce * Time.deltaTime * 500);
        }

        if (fastFall) {
            rb.AddForce(-0.5f * transform.up * hvForce * Time.deltaTime * 500);
        }

        if (dash) {
            dash = false; dashTime = 0.25f;
            rb.linearVelocityX /= 5.0f;
            rb.linearVelocityY /= 5.0f;
            rb.AddForce(mDir * dashForce, ForceMode2D.Impulse);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint")) {
            
            if (collision.GetComponent<Checkpoint>().checkNum > checkNum) {
                checkNum = collision.GetComponent<Checkpoint>().checkNum;
                //respawn = new Vector2(collision.GetComponent<Checkpoint>().respawnX, collision.GetComponent<Checkpoint>().respawnY);
                respawn = collision.GetComponent<Checkpoint>().respawn;
                fuel = 100.0f; engDead = false;
            }
        } else if (collision.gameObject.CompareTag("Orb")) {
            var orb = collision.gameObject.GetComponent<Orb>();
            if (orb.type == "Fuel" && orb.currCD <= 0) {
                fuel += 80.0f; engDead = false;
                //orb.currCD = orb.cd;
            }
        } else if (collision.gameObject.CompareTag("End")) {
            bhImmune = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Hurt")) {
            Hurt();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Blackhole" && !bhImmune) {
            Hurt();
        }
    }

    void Hurt() {
        if (!bhImmune) {
            bhImmune = true;
            rb.linearVelocityY = 0; rb.linearVelocityX = 0; rb.linearDamping = initLinDamp;
            dashTime = 0.0f;
            gameObject.SetActive(false);
            gameManager.DeathTransition();
            fuel = 100.0f;
        }
    }

    public void playRespawn() {
        transform.position = respawn; fuel = 100.0f; engDead = false; bhImmune = false;
        hover = false; dash = false; dashTime = 0.0f;
        ChangeSprite(); cc.size = ccNormSz;
    }

    void ChangeSprite() {
        string spriteName = "slosh_";

        if (dashTime > 0) {spriteName += "D2";}
        else {
            if (hover) {spriteName += "F"; }
            spriteName += direct;
        }
        spriteName += "_0";

        /*if (hover) {spriteName += "F";}
        else if (dashTime < 0) {spriteName += "D";}
        spriteName += direct + "_0";*/

        sr.sprite = spriteDict[spriteName];
    }

    public float GetFuel() {
        return fuel;
    }

    public Vector2 GetRespawn() {
        return respawn;
    }

    public void ToggleImmune(string type) {
        if (type == "bh") {
            if (bhImmune) {
                
            }
        }
    }
}
