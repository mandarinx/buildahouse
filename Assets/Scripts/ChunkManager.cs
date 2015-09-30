using UnityEngine;
using System.Collections.Generic;

public static class ChunkManager {

    static public Dictionary<int, Chunk>    chunks { get { return _chunks; }}
    static public int                       chunkSize { get { return 8; }}
    static public int                       blockSize { get { return 1; }}
    static public Vector3                   pivot { get { return _pivot; }}

    static private Vector3                  _pivot = new Vector3(0.5f, 0f, 0.5f);
    static private Dictionary<int, Chunk>   _chunks = new Dictionary<int, Chunk>();
    static private int                      chunkDim = chunkSize * blockSize;

    static public Chunk GetChunk(Point3 worldBlockCoord) {
        Chunk chunk;
        chunks.TryGetValue(GetHash(worldBlockCoord.ToChunkCoord()), out chunk);
        return chunk;
    }

    static public Voxel GetBlock(int worldX, int worldY, int worldZ) {
        return GetBlock(new Point3(worldX, worldY, worldZ));
    }

    static public Voxel GetBlock(Vector3 worldCoord) {
        return GetBlock(Point3.ToWorldBlockCoord(worldCoord));
    }

    static public Voxel GetBlock(Point3 worldBlockCoord) {
        Chunk chunk = GetChunk(worldBlockCoord);
        return chunk != null
            ? chunk.Get(worldBlockCoord.ToLocalBlockCoord())
            : null;
    }

    // Add overrides for Vector3 and chunk coord?
    static public Chunk AddChunk(Point3 worldBlockCoord) {
        Chunk chunk = GetChunk(worldBlockCoord);
        Point3 chunkCoord = worldBlockCoord.ToChunkCoord();
        if (chunk == null) {
            chunk = new Chunk(chunkSize, chunkCoord.ToVector3() * chunkSize * blockSize);
            chunks.Add(GetHash(chunkCoord), chunk);
        }
        return chunk;
    }

    static public int GetHash(Point3 chunkCoord) {
        return (int)(chunkCoord.x + chunkCoord.z * chunkSize + chunkCoord.y * chunkSize * chunkSize);
    }

    // Calculates chunk coordinate from world space block coordinate
    static public Point3 ToChunkCoord(Point3 worldBlockCoord) {
        return new Point3(
            ChunkCoordAxis(worldBlockCoord.x, pivot.x),
            ChunkCoordAxis(worldBlockCoord.y, pivot.y),
            ChunkCoordAxis(worldBlockCoord.z, pivot.z)
        );
    }

    static private float ChunkCoordAxis(float axisvalue, float pivot) {
        return Mathf.Floor((axisvalue + chunkDim * pivot) / chunkDim);
    }

    // Snaps world space coordinate to block coordinates
    static public float BlockIndexAxis(float value) {
        return Mathf.Floor(value / blockSize);
    }

    // Converts world space coordinate to local space relative to chunk size
    static public Point3 ToLocalBlockCoord(Point3 worldBlockCoord) {
        return new Point3(
            LocalCoordAxis(worldBlockCoord.x, pivot.x),
            LocalCoordAxis(worldBlockCoord.y, pivot.y),
            LocalCoordAxis(worldBlockCoord.z, pivot.z)
        );
    }

    static private float LocalCoordAxis(int axisValue, float pivot) {
        float value = (axisValue + chunkDim * pivot) % chunkDim;
        return value < 0 ? value - (int)Mathf.Floor(value / (float)chunkDim) * chunkDim : value;
    }

}