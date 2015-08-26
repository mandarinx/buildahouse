using UnityEngine;

public class Point3 {

    public int x { get; private set; }
    public int y { get; private set; }
    public int z { get; private set; }

    public Point3() {}

    public Point3(double x, double y, double z) {
        this.x = (int)x;
        this.y = (int)y;
        this.z = (int)z;
    }

    public Vector3 ToVector3() {
        return new Vector3(x, y, z);
    }

    override public string ToString() {
        return "Point3 { x: "+x+" y: "+y+" z: "+z+" }";
    }
}