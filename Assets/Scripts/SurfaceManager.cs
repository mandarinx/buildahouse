using UnityEngine;
using System;

public class SurfaceManager {

    private ChunkManager    chunkManager;
    private Voxel[]         neighbours;

    public SurfaceManager(ChunkManager cm) {
        chunkManager = cm;
        neighbours = new Voxel[6];
    }

    public void FindSurface(ref Voxel block, Point3 worldCoord) {
        int id = GetID(block, worldCoord);

        // use id to find rotation
            // set rotation on block

        // use theme, type and variant to get mesh name
        // or add id to a return object

        // run neighbours through same process,
            // except don't check the neighbour's neighbours
    }

    private int GetID(Voxel block, Point3 worldCoord) {
        Array.Clear(neighbours, 0, neighbours.Length);
        int i = 0;
        int id = 0;

        Voxel n1 = GetNeighbour(worldCoord,  1,  0,  0);
        if (n1 != null) {
            neighbours[i++] = n1;
            id += 1;
        }

        Voxel n2 = GetNeighbour(worldCoord, -1,  0,  0);
        if (n2 != null) {
            neighbours[i++] = n2;
            id += 2;
        }

        Voxel n3 = GetNeighbour(worldCoord,  0,  1,  0);
        if (n3 != null) {
            neighbours[i++] = n3;
            id += 4;
        }

        Voxel n4 = GetNeighbour(worldCoord,  0, -1,  0);
        if (n4 != null) {
            neighbours[i++] = n4;
            id += 8;
        }

        Voxel n5 = GetNeighbour(worldCoord,  0,  0,  1);
        if (n5 != null) {
            neighbours[i++] = n5;
            id += 16;
        }

        Voxel n6 = GetNeighbour(worldCoord,  0,  0, -1);
        if (n6 != null) {
            neighbours[i++] = n6;
            id += 32;
        }

        return id;
    }

    private Voxel GetNeighbour(Point3 worldCoord, int x, int y, int z) {
        return chunkManager.GetBlock(worldCoord.x + x, worldCoord.y + y, worldCoord.z + z);
    }
}
