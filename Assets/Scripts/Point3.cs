using UnityEngine;

public class Point3 {

    public int x { get; set; }
    public int y { get; set; }
    public int z { get; set; }

    public Point3() {}

    public Point3(double x, double y, double z) {
        this.x = (int)x;
        this.y = (int)y;
        this.z = (int)z;
    }

    public Vector3 ToVector3() {
        return new Vector3(x, y, z);
    }

    public void CopyTo(ref Vector3 v3) {
        v3.x = x;
        v3.y = y;
        v3.z = z;
    }

    static public Point3 zero {
        get { return new Point3(); }
    }

    override public string ToString() {
        return "Point3 { x: "+x+" y: "+y+" z: "+z+" }";
    }
}