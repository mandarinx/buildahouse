using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public ChunkManager     chunkManager;

    void Awake() {
        chunkManager = new ChunkManager();
        Callbacks.OnSelectionData += OnSelectionData;
    }

    private void OnSelectionData(float[] data) {
        int width = (int)Mathf.Abs(data[2]);
        int length = (int)Mathf.Abs(data[3]);

        for (int x=0; x<width; x++) {
            int xpos = (int)data[0];
            int ypos = (int)data[1];
            xpos += x * (int)Mathf.Sign(data[2]);

            for (int y=0; y<length; y++) {
                ypos += y * (int)Mathf.Sign(data[3]);
                Vector3 blockCoord = chunkManager.GetBlockCoord(new Vector3(xpos, 0f, ypos));
                chunkManager.SetBlock(blockCoord, 1);
            }

        }
    }
}
