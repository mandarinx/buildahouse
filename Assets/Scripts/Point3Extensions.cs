using UnityEngine;

static public class Point3Extensions {

    static public Vector3 ToVector3(this Point3 p3) {
        return Point3.ToVector3(p3);
    }

    // Casts world coordinate to global block coordinate
    static public Point3 ToWorldBlockCoord(this Point3 p3, Vector3 worldCoord) {
        return Point3.ToWorldBlockCoord(worldCoord);
    }

    // Converts world space block coord to local space relative
    // to the chunk that owns the block
    static public Point3 ToLocalBlockCoord(this Point3 p3) {
        return ChunkManager.ToLocalBlockCoord(p3);
    }

    // Returns chunk coord based on local space block coord
    static public Point3 ToChunkCoord(this Point3 p3) {
        return ChunkManager.ToChunkCoord(p3);
    }

}