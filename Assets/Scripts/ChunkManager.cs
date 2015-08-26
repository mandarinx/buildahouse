using UnityEngine;
using System.Collections.Generic;

public class ChunkManager {

    public Dictionary<int, Chunk>   chunks { get; private set; }
    public int                      chunkSize { get { return 8; }}
    public int                      blockSize { get { return 1; }}
    public Vector3                  pivot { get; private set; }
    private int                     chunkDim;

    public ChunkManager() {
        pivot = new Vector3(0.5f, 0f, 0.5f);
        chunkDim = chunkSize * blockSize;
        chunks = new Dictionary<int, Chunk>();
    }

    public Vector3 GetBlockCoord(Vector3 worldCoord) {
        return new Vector3(Mathf.Floor(worldCoord.x) + chunkDim * pivot.x,
                           Mathf.Floor(worldCoord.y) + chunkDim * pivot.y,
                           Mathf.Floor(worldCoord.z) + chunkDim * pivot.z);
    }

    public void SetBlock(Vector3 blockCoord, int value) {
        Vector3 chunkCoord = GetChunkCoord(blockCoord);
        int hash = GetHash(chunkCoord);
        Chunk chunk;
        chunks.TryGetValue(hash, out chunk);
        if (chunk == null) {
            chunk = new Chunk(chunkSize, chunkCoord * chunkSize * blockSize);
            chunks.Add(hash, chunk);
        }
        chunk.Set(blockCoord, value);
    }

    public Vector3 GetChunkCoord(Vector3 blockCoord) {
        return new Vector3(Mathf.Floor(blockCoord.x / chunkDim),
                           Mathf.Floor(blockCoord.y / chunkDim),
                           Mathf.Floor(blockCoord.z / chunkDim));
    }

    public int GetHash(Vector3 chunkCoord) {
        return (int)(chunkCoord.x + chunkCoord.z * chunkSize + chunkCoord.y * chunkSize * chunkSize);
    }

}