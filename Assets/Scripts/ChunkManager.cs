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
    public Point3 GetWorldBlockCoord(Vector3 worldCoord) {
        return new Point3(Mathf.Floor(worldCoord.x / blockSize),
                          Mathf.Floor(worldCoord.y / blockSize),
                          Mathf.Floor(worldCoord.z / blockSize));
    }

    // Converts world space block coord to local space relative
    // to the chunk that owns the block
    public Point3 GetLocalBlockCoord(Point3 worldBlocCoord) {
        float x = (worldBlocCoord.x + chunkDim * pivot.x) % chunkDim;
        float y = (worldBlocCoord.y + chunkDim * pivot.y) % chunkDim;
        float z = (worldBlocCoord.z + chunkDim * pivot.z) % chunkDim;

        return new Point3(
            x < 0 ? x - ((int)Mathf.Floor(x / (float)chunkDim) * chunkDim) : x,
            y < 0 ? y - ((int)Mathf.Floor(y / (float)chunkDim) * chunkDim) : y,
            z < 0 ? z - ((int)Mathf.Floor(z / (float)chunkDim) * chunkDim) : z
        );
    }

    // Returns chunk coord based on local space block coord
    public Point3 GetChunkCoord(Point3 worldBlockCoord) {
        return new Point3(Mathf.Floor((worldBlockCoord.x + chunkDim * pivot.x) / chunkDim),
                          Mathf.Floor((worldBlockCoord.y + chunkDim * pivot.y) / chunkDim),
                          Mathf.Floor((worldBlockCoord.z + chunkDim * pivot.z) / chunkDim));
    }

    public Voxel GetBlock(int worldX, int worldY, int worldZ) {
        return GetBlock(new Point3(worldX, worldY, worldZ));
    }

    public Voxel GetBlock(Vector3 worldCoord) {
        return GetBlock(GetWorldBlockCoord(worldCoord));
    }

    public Voxel GetBlock(Point3 worldBlockCoord) {
        Point3 chunkCoord = GetChunkCoord(worldBlockCoord);
        int hash = GetHash(chunkCoord);
        Chunk chunk;
        chunks.TryGetValue(hash, out chunk);
        if (chunk == null) {
            return null;
        }

        return chunk.Get(GetLocalBlockCoord(worldBlockCoord));
    }

    // Add overrides for Vector3 and chunk coord?
    public Chunk AddChunk(Point3 worldBlockCoord) {
        Point3 chunkCoord = GetChunkCoord(worldBlockCoord);
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