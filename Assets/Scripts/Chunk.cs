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

    //  public void Set(Vector3 blockCoord, int value) {
    //      //  Debug.Log("Set x: "+(int)(blockCoord.x - worldCoord.x)+" "+
    //      //            "y: "+(int)(blockCoord.y - worldCoord.y)+" "+
    //      //            "z: "+(int)(blockCoord.z - worldCoord.z));
    //      voxels[(int)(blockCoord.x - worldCoord.x),
    //             (int)(blockCoord.y - worldCoord.y),
    //             (int)(blockCoord.z - worldCoord.z)].data = value;
    //  }

    //  public Voxel GetBlock(Vector3 blockCoord) {
    //      Debug.Log("GetBlock: "+blockCoord);

    //      // This fails when coordinates are negative. Coords should be
    //      // offset, or something, before accessing the array elements

    //      Voxel voxel = voxels[(int)(blockCoord.x - worldCoord.x),
    //                           (int)(blockCoord.y - worldCoord.y),
    //                           (int)(blockCoord.z - worldCoord.z)];
    //      if (voxel == null) {
    //          voxel = new Voxel();
    //      }
    //      return voxel;
    //  }

    //  public void Fill() {
    //      int x = 0, y = 0, z = 0;
    //      for (int i=0; i<voxels.Length; i++) {
    //          Set(x, y, z, 1);
    //      }
    //  }
}
