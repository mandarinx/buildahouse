using UnityEngine;

public class Point3 {

    public int x { get; set; }
    public int y { get; set; }
    public int z { get; set; }

    public Point3() {}

    public Point3(float x, float y, float z) {
        this.x = (int)x;
        this.y = (int)y;
        this.z = (int)z;
    }

    static public Vector3 ToVector3(Point3 p3) {
        return new Vector3(p3.x, p3.y, p3.z);
    }

    // Casts world coordinate to global block coordinate
    static public Point3 ToWorldBlockCoord(Vector3 worldCoord) {
        return new Point3(ChunkManager.BlockIndexAxis(worldCoord.x),
                          ChunkManager.BlockIndexAxis(worldCoord.y),
                          ChunkManager.BlockIndexAxis(worldCoord.z));
    }

    public static Point3 operator +(Point3 a, Point3 b)  {
        return new Point3(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static Point3 operator -(Point3 a, Point3 b)  {
        return new Point3(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static Point3 operator *(Point3 a, int v)  {
        return new Point3(a.x * v, a.y * v, a.z * v);
    }

    static public Point3 zero {
        get { return new Point3(); }
    }

    static public Point3 one {
        get { return new Point3(1, 1, 1); }
    }

    static public Point3 up {
        get { return new Point3(0, 1, 0); }
    }

    static public Point3 right {
        get { return new Point3(1, 0, 0); }
    }

    static public Point3 forward {
        get { return new Point3(0, 0, 1); }
    }

    override public string ToString() {
        return "Point3 { x: "+x+" y: "+y+" z: "+z+" }";
    }
}