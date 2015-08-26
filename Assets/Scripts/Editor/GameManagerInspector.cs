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
            chunkCoord = gm.chunkManager.GetChunkCoord(blockCoord);
        }
        EditorGUILayout.Vector3Field("Chunk coord", chunkCoord);

        if (GUILayout.Button("Add chunk")) {
            //  Vector3 blockCoord = gm.chunkManager.GetBlockCoord(blockCoord);
            gm.chunkManager.SetBlock(blockCoord, 1);
        }
    }

    void OnSceneGUI() {
        if (!Application.isPlaying) {
            return;
        }

        foreach (KeyValuePair<int, Chunk> pair in gm.chunkManager.chunks) {
            Vector3 dot = Vector3.zero;

            for (int y=0; y<gm.chunkManager.chunkSize + 1; y++) {
                dot.y = CalcPos(y, gm.chunkManager.pivot.y, pair.Value.worldCoord.y);

                for (int z=0; z<gm.chunkManager.chunkSize + 1; z++) {
                    dot.z = CalcPos(z, gm.chunkManager.pivot.z, pair.Value.worldCoord.z);

                    for (int x=0; x<gm.chunkManager.chunkSize + 1; x++) {
                        dot.x = CalcPos(x, gm.chunkManager.pivot.x, pair.Value.worldCoord.x);
                        Handles.CubeCap(0, dot, Quaternion.identity, 0.1f);
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
