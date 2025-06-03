using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    public string text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (transform.rotation.z == 0) {General.PointToBH(this.gameObject);}
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            GameManager.instance.ShowText(text);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && GameManager.instance.panelText.text == "") {
            GameManager.instance.ShowText(text);
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            GameManager.instance.HideText();
        }
    }
}
