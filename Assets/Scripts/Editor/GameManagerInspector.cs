using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(GameManager))]
public class GameManagerInspector : Editor {

    private GameManager gm;
    private Vector3     blockCoord;
    private Vector3     chunkCoord;

    void OnEnable() {
        gm = target as GameManager;
    }

    override public void OnInspectorGUI() {
        DrawDefaultInspector();

        Vector3 newBlockCoord = EditorGUILayout.Vector3Field("Block coord", blockCoord);
        if (newBlockCoord != blockCoord) {
            blockCoord = newBlockCoord;
            //  chunkCoord = gm.chunkManager.GetChunkCoord(blockCoord);
        }
        EditorGUILayout.Vector3Field("Chunk coord", chunkCoord);

        if (GUILayout.Button("Add chunk")) {
            //  gm.chunkManager.AddChunk(blockCoord);
        }
    }

    void OnSceneGUI() {
        if (!Application.isPlaying) {
            return;
        }

        Handles.color = new Color(1f, 1f, 1f, 0.05f);

        foreach (KeyValuePair<int, Chunk> pair in gm.chunkManager.chunks) {
            Vector3 dot = Vector3.zero;

            for (int y=0; y<gm.chunkManager.chunkSize; y++) {
                dot.y = CalcPos(y, gm.chunkManager.pivot.y, pair.Value.worldCoord.y);

                for (int z=0; z<gm.chunkManager.chunkSize; z++) {
                    dot.z = CalcPos(z, gm.chunkManager.pivot.z, pair.Value.worldCoord.z);

                    for (int x=0; x<gm.chunkManager.chunkSize; x++) {
                        dot.x = CalcPos(x, gm.chunkManager.pivot.x, pair.Value.worldCoord.x);

                        //  Debug.Log("Get block: "+dot);
                        //  Voxel block = gm.chunkManager.GetBlock(dot);
                        //  if (block != null) {
                        //      int blockType = DataParser.GetBlockType(block.data);
                        //      if (blockType == 1) {
                        //          Debug.Log("Block "+dot+" is type 1");
                        //          Handles.color = new Color(0f, 1f, 0f, 0.1f);
                        //      }
                        //  }
                        Handles.CubeCap(0, dot, Quaternion.identity, 1f);
                        Handles.color = new Color(1f, 1f, 1f, 0.05f);
                    }
                }
            }
        }
    }

    private float CalcPos(float index, float offset, float worldOffset) {
        return index * gm.chunkManager.blockSize -
               offset * gm.chunkManager.blockSize * gm.chunkManager.chunkSize +
               worldOffset;
    }

    void DrawLines(float chunkDim) {
        // I forgot to take chunk.worldpos into account
        Vector3 start = Vector3.zero;
        Vector3 end = Vector3.zero;

        for (int y=0; y<gm.chunkManager.chunkSize + 1; y++) {
            start.y = y * gm.chunkManager.blockSize - gm.chunkManager.pivot.y * chunkDim;
            end.y = start.y;

            start.z = gm.chunkManager.pivot.z * chunkDim * -1;
            end.z = start.z + chunkDim;

            for (int x=0; x<gm.chunkManager.chunkSize + 1; x++) {
                start.x = end.x = (gm.chunkManager.blockSize * x) - (chunkDim * gm.chunkManager.pivot.x);
                Handles.DrawLine(start, end);
            }

            start.x = gm.chunkManager.pivot.x * chunkDim * -1;
            end.x = start.x + chunkDim;

            for (int z=0; z<gm.chunkManager.chunkSize + 1; z++) {
                start.z = end.z = (gm.chunkManager.blockSize * z) - (chunkDim * gm.chunkManager.pivot.z);
                Handles.DrawLine(start, end);
            }
        }
    }
}
