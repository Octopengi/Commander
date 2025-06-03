using UnityEngine;
using UnityEngine.UIElements;

public static class General {
    public static void PointToBH(GameObject item) {
        GameObject blackHole = GameObject.Find("Blackhole");
        var dir = blackHole.transform.position - item.transform.position;
        dir.Normalize();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        item.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }
    
    public static void PointToBH(GameObject item, float mult, bool absV = false) {
        GameObject blackHole = GameObject.Find("Blackhole");
        var dir = blackHole.transform.position - item.transform.position;
        dir.Normalize();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        if (absV) {item.transform.rotation = Quaternion.AngleAxis(angle + 90 * mult, Vector3.forward);}
        else {item.transform.rotation = Quaternion.AngleAxis(angle + 90 * mult, Vector3.forward);}
    }

}