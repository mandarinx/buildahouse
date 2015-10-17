using UnityEngine;

static public class Vector3Extensions {

    static public Vector3 Round(this Vector3 v3) {
        v3.x = Mathf.Round(v3.x);
        v3.y = Mathf.Round(v3.y);
        v3.z = Mathf.Round(v3.z);
        return v3;
    }

    static public Vector3 Floor(this Vector3 v3) {
        v3.x = Mathf.Floor(v3.x);
        v3.y = Mathf.Floor(v3.y);
        v3.z = Mathf.Floor(v3.z);
        return v3;
    }

}