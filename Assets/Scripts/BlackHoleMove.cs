using System.Collections.Generic;
using UnityEngine;

public class BlackHoleMove : MonoBehaviour
{
    public List<Vector3> spots;
    public float moveCD;
    public float spd = 0.5f;

    int spot = -1;
    float currMoveCD = 0.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (spots.Count != 0) {transform.position = spots[0]; spot = 0; currMoveCD = moveCD;} 
    }

    // Update is called once per frame
    void Update() {
        if (currMoveCD <= 0 && spot != -1) {
            currMoveCD = moveCD;
            spot += 1;
            if (spot > spots.Count - 1) spot = 0;
        }
    }

    void FixedUpdate() {
        if (currMoveCD > 0 && spot != -1) {
            currMoveCD -= Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, spots[spot], Time.deltaTime * spd);
        }
    }
}
