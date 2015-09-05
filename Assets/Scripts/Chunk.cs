using UnityEngine;

public class Chunk {

    public Vector3      worldCoord { get; private set; }
    private Voxel[,,]   voxels;

    public Chunk(int size, Vector3 worldCoord) {
        voxels = new Voxel[size, size, size];
        this.worldCoord = worldCoord;
    }

    public Voxel Get(int x, int y, int z) {
        return voxels[x, y, z];
    }

    public Voxel Get(Point3 localBlockCoord) {
        return voxels[localBlockCoord.x,
                      localBlockCoord.y,
                      localBlockCoord.z];
    }

    public Voxel Add(Point3 localBlockCoord) {
        voxels[localBlockCoord.x,
               localBlockCoord.y,
               localBlockCoord.z] = new Voxel();
        return Get(localBlockCoord);
    }
}
