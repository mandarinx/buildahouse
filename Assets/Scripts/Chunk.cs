using UnityEngine;

public class Chunk {

    public Vector3 worldCoord { get; private set; }

    private int[,,] voxels;

    public Chunk(int size, Vector3 worldCoord) {
        voxels = new int[size, size, size];
        this.worldCoord = worldCoord;
    }

    public void Set(int x, int y, int z, int value) {
        voxels[x, y, z] = value;
    }

    public void Set(Vector3 blockCoord, int value) {
        //  Debug.Log("Set x: "+(int)(blockCoord.x - worldCoord.x)+" "+
        //            "y: "+(int)(blockCoord.y - worldCoord.y)+" "+
        //            "z: "+(int)(blockCoord.z - worldCoord.z));
        voxels[(int)(blockCoord.x - worldCoord.x),
               (int)(blockCoord.y - worldCoord.y),
               (int)(blockCoord.z - worldCoord.z)] = value;
    }

    public void Fill() {
        int x = 0, y = 0, z = 0;
        for (int i=0; i<voxels.Length; i++) {
            Set(x, y, z, 1);
        }
    }
}
