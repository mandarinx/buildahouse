﻿using UnityEngine;
using Mandarin;

public class GameManager : MonoBehaviour {

    [HideInInspector]
    public BlockManager     blockManager;

    void Awake() {
        //  Callbacks.OnSelectionData += OnSelectionData;

        GO.Create()
            .SetName("BlockManager")
            .AddComponent<BlockManager>((bm) => {
                blockManager = bm;
                bm.Init();
            });

        //  Vector3 v3 = Vector3.zero;
        //  Point3 p3 = Point3.ToWorldBlockCoord(v3).
    }

    //  private void OnSelectionData(float[] data) {
    //      int width = (int)Mathf.Abs(data[2]) + 1;
    //      int length = (int)Mathf.Abs(data[3]) + 1;

    //      //  Debug.Log("Selection width: "+width+" length: "+length);

    //      for (int x=0; x<width; x++) {
    //          int xpos = 0;
    //          int ypos = 0;

    //          xpos = (int)data[0] + x * (int)Mathf.Sign(data[2]);

    //          for (int y=0; y<length; y++) {
    //              ypos = (int)data[1] + y * (int)Mathf.Sign(data[3]);
    //              //  Vector3 blockCoord = chunkManager.GetBlockCoord(new Vector3(xpos, 0f, ypos));
    //              //  Voxel block = chunkManager.GetBlock(blockCoord);
    //              //  if (block == null) {
    //              //      Chunk chunk = chunkManager.AddChunk(blockCoord);
    //              //      block = chunkManager.GetBlock(blockCoord);
    //              //  }
    //              //  DataParser.SetBlockType(ref block.data, 3);
    //          }

    //      }
    //  }
}
