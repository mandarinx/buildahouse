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
                dot.y = GetPos(pair.Value.worldCoord.y, gm.chunkManager.pivot.y, y);

                for (int z=0; z<gm.chunkManager.chunkSize; z++) {
                    dot.z = GetPos(pair.Value.worldCoord.z, gm.chunkManager.pivot.z, z);

                    for (int x=0; x<gm.chunkManager.chunkSize; x++) {
                        dot.x = GetPos(pair.Value.worldCoord.x, gm.chunkManager.pivot.x, x);

                        Voxel block = gm.chunkManager.GetBlock(dot);
                        if (block != null) {
                        //      //  Debug.Log("block at "+dot);
                        //      //  int blockType = DataParser.GetBlockType(block.data);
                        //      //  if (blockType == 1) {
                                Handles.color = new Color(0f, 1f, 0f, 0.5f);
                        //      //  }
                        } else {
                            Handles.color = new Color(1f, 1f, 1f, 0.05f);
                        }
                        Handles.CubeCap(0, dot, Quaternion.identity, 1f);
                        //  Debug.Log("handles color: "+Handles.color);
                        //  Handles.color = new Color(Random.value, Random.value, Random.value, 0.5f);
                        //  Handles.CubeCap(0, dot, Quaternion.identity, 1f);
                    }
                }
            }
        }
    }

    private float GetPos(float worldPos, float pivot, float index) {
        return (worldPos - (gm.chunkManager.blockSize * gm.chunkManager.chunkSize * pivot)) +
               (index * gm.chunkManager.blockSize) +
               // Add 0.5f due to the pivot of the handle being in the center
               0.5f;
    }

}
