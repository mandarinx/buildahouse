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

    // Casts world coordinate to global block coordinate
    public Point3 GetBlockCoord(Vector3 worldCoord) {
        return new Point3(Mathf.Floor(worldCoord.x / blockSize),
                          Mathf.Floor(worldCoord.y / blockSize),
                          Mathf.Floor(worldCoord.z / blockSize));

        //  return new Vector3(Mathf.Floor(worldCoord.x) + chunkDim * pivot.x,
        //                     Mathf.Floor(worldCoord.y) + chunkDim * pivot.y,
        //                     Mathf.Floor(worldCoord.z) + chunkDim * pivot.z);
    }

    public Point3 GetLocalBlockCoord(Point3 blockCoord) {
        float x = (blockCoord.x + chunkDim * pivot.x) % chunkDim;
        float y = (blockCoord.y + chunkDim * pivot.y) % chunkDim;
        float z = (blockCoord.z + chunkDim * pivot.z) % chunkDim;

        return new Point3(
            x < 0 ? x - ((int)Mathf.Floor(x / (float)chunkDim) * chunkDim) : x,
            y < 0 ? y - ((int)Mathf.Floor(y / (float)chunkDim) * chunkDim) : y,
            z < 0 ? z - ((int)Mathf.Floor(z / (float)chunkDim) * chunkDim) : z
        );
    }

    public Point3 GetChunkCoord(Point3 blockCoord) {
        return new Point3(Mathf.Floor((blockCoord.x + chunkDim * pivot.x) / chunkDim),
                          Mathf.Floor((blockCoord.y + chunkDim * pivot.y) / chunkDim),
                          Mathf.Floor((blockCoord.z + chunkDim * pivot.z) / chunkDim));
        //  return new Vector3(Mathf.Floor(blockCoord.x / chunkDim),
        //                     Mathf.Floor(blockCoord.y / chunkDim),
        //                     Mathf.Floor(blockCoord.z / chunkDim));
    }

    // Use a getChunk to get the chunk, then addChunk if get returns null
    // get block via block coordinate

    //  public Chunk GetChunk(Vector3 )
    public Voxel GetBlock(Point3 blockCoord) {
        Point3 chunkCoord = GetChunkCoord(blockCoord);
        int hash = GetHash(chunkCoord);
        Debug.Log("blockCoord: "+blockCoord+" = chunkCoord: "+chunkCoord+" hash: "+hash);
        Chunk chunk;
        chunks.TryGetValue(hash, out chunk);
        if (chunk == null) {
            return null;
        }

        Voxel voxel = chunk.Get((int)(blockCoord.x - chunk.worldCoord.x),
                                (int)(blockCoord.y - chunk.worldCoord.y),
                                (int)(blockCoord.z - chunk.worldCoord.z));

        return voxel;
    }

    public Chunk AddChunk(Point3 blockCoord) {
        Point3 chunkCoord = GetChunkCoord(blockCoord);
        int hash = GetHash(chunkCoord);
        Chunk chunk;
        chunks.TryGetValue(hash, out chunk);
        if (chunk == null) {
            chunk = new Chunk(chunkSize, chunkCoord.ToVector3() * chunkSize * blockSize);
            chunks.Add(hash, chunk);
        }
        return chunk;
    }

    public int GetHash(Point3 chunkCoord) {
        return (int)(chunkCoord.x + chunkCoord.z * chunkSize + chunkCoord.y * chunkSize * chunkSize);
    }

}