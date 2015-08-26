using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public ChunkManager     chunkManager;

    //  public Vector3          raycasthit;
    //  public Vector3          blockCoord;
    void Awake() {
        chunkManager = new ChunkManager();
        //  blockCoord = chunkManager.GetBlockCoord(raycasthit);
        //  chunkManager.SetBlock(blockCoord, 1);
    }

    void Update() {
        // raycaster => vector3
        // display build block, snapped to voxel grid
            // get global voxel coord
            // multiply vector by blocksize

        // click:
        // chunks.GetBlock(coord).SetType(int)
    }
}
