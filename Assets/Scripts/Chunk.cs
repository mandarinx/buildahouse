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

    public void Set(int x, int y, int z, Voxel voxel) {
        if (voxels.GetLength(0) < x ||
            voxels.GetLength(1) < y ||
            voxels.GetLength(2) < z) {
            return;
        }
        voxels[x, y, z] = voxel;
    }

    public void Set(Point3 coord, Voxel voxel) {
        Set(coord.x, coord.y, coord.z, voxel);
    }

    public Voxel Get(Point3 localBlockCoord) {
        return voxels[localBlockCoord.x,
                      localBlockCoord.y,
                      localBlockCoord.z];
    }

    public Voxel AddBlock(Point3 localBlockCoord) {
        voxels[localBlockCoord.x,
               localBlockCoord.y,
               localBlockCoord.z] = new Voxel();
        return Get(localBlockCoord);
    }

    public void RemoveBlock(Point3 localCoord) {
        Set(localCoord, null);
    }
}
