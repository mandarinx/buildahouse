using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public ChunkManager     chunkManager;

    void Awake() {
        chunkManager = new ChunkManager();
        Callbacks.OnSelectionData += OnSelectionData;
    }

    private void OnSelectionData(float[] data) {
        int width = (int)Mathf.Abs(data[2]) + 1;
        int length = (int)Mathf.Abs(data[3]) + 1;

        //  Debug.Log("Selection width: "+width+" length: "+length);

        for (int x=0; x<width; x++) {
            int xpos = 0;
            int ypos = 0;

            xpos = (int)data[0] + x * (int)Mathf.Sign(data[2]);

            for (int y=0; y<length; y++) {
                ypos = (int)data[1] + y * (int)Mathf.Sign(data[3]);
                //  Vector3 blockCoord = chunkManager.GetBlockCoord(new Vector3(xpos, 0f, ypos));
                //  Voxel block = chunkManager.GetBlock(blockCoord);
                //  if (block == null) {
                //      Chunk chunk = chunkManager.AddChunk(blockCoord);
                //      block = chunkManager.GetBlock(blockCoord);
                //  }
                //  DataParser.SetBlockType(ref block.data, 3);
            }

        }
    }
}
