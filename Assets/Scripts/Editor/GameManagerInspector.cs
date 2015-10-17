using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(GameManager))]
public class GameManagerInspector : Editor {

    private GameManager gm;

    void OnEnable() {
        gm = target as GameManager;
    }

    override public void OnInspectorGUI() {
        DrawDefaultInspector();
    }

    void OnSceneGUI() {
        if (!Application.isPlaying) {
            return;
        }

        Handles.color = new Color(1f, 1f, 1f, 0.05f);

        foreach (KeyValuePair<int, Chunk> pair in ChunkManager.chunks) {
            Vector3 dot = Vector3.zero;

            for (int y=0; y<ChunkManager.chunkSize; y++) {
                dot.y = GetPos(pair.Value.worldCoord.y, ChunkManager.pivot.y, y);

                for (int z=0; z<ChunkManager.chunkSize; z++) {
                    dot.z = GetPos(pair.Value.worldCoord.z, ChunkManager.pivot.z, z);

                    for (int x=0; x<ChunkManager.chunkSize; x++) {
                        dot.x = GetPos(pair.Value.worldCoord.x, ChunkManager.pivot.x, x);

                        Voxel block = ChunkManager.GetBlock(dot);
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
                    }
                }
            }
        }
    }

    private float GetPos(float worldPos, float pivot, float index) {
        return (worldPos - (ChunkManager.blockSize * ChunkManager.chunkSize * pivot)) +
               (index * ChunkManager.blockSize) +
               // Add 0.5f due to the pivot of the handle being in the center
               0.5f;
    }

}
