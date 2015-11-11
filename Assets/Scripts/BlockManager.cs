using UnityEngine;
using UnityEngine.Rendering;
using System;
using System.Collections.Generic;
using Mandarin;
using HyperGames;

public class BlockManager : MonoBehaviour {

    private const string                    objNameFormat = "{0}_{1}_{2}";
    private Material                        voxelMat;
    private Mesh                            voxelMesh;
    private Dictionary<int, GameObject>     chunks;
    static private Point3[]                 checkSides = new Point3[] {
                                                Point3.forward,         Point3.right,
                                                Point3.forward * -1,    Point3.right * -1,
                                                Point3.up,              Point3.up * -1
                                            };

    public void Init() {
        chunks = new Dictionary<int, GameObject>();

        voxelMesh = new MeshBuilder()
            .CreateCube(ChunkManager.blockSize,
                        ChunkManager.blockSize,
                        ChunkManager.blockSize,
                        new Vector3(0.5f, 0.5f, 0.5f))
            .GetMesh();

        voxelMat = Resources.Load("Dev") as Material;

        Messenger.AddListener<PlacedBlock>(OnPlacedBlock);
        Messenger.AddListener<RemoveBlock>(OnRemoveBlock);
    }

    private void OnRemoveBlock(RemoveBlock msg) {
        Point3 worldCoord = msg.worldCoord;
        Chunk chunk = ChunkManager.GetChunk(worldCoord);
        if (chunk != null) {
            chunk.RemoveBlock(worldCoord.ToLocalBlockCoord());
        }

        int hash = ChunkManager.GetHash(worldCoord.ToChunkCoord());
        string name = Name(worldCoord);

        // TODO: Fix the deleting of gameobjects. I'd rather use a pool.
        // Casts an error when hash doesn't exist, like when clicking
        // on the ground
        GameObject container = chunks[hash];
        foreach (Transform child in container.transform) {
            if (child.name == name) {
                Destroy(child.gameObject);
            }
        }

        UpdateNeighbours(GetNeighbours(worldCoord), worldCoord);
    }

    private void OnPlacedBlock(PlacedBlock msg) {
        Point3 worldCoord = msg.worldCoord;
        Voxel block = ChunkManager.GetBlock(worldCoord);
        if (block == null) {
            block = ChunkManager
                .AddChunk(worldCoord)
                .AddBlock(worldCoord.ToLocalBlockCoord());
        }

        int hash = ChunkManager.GetHash(worldCoord.ToChunkCoord());
        block.SetBlockType(msg.type);

        // Get neighbours, pass to surface manager
        Voxel[] neighbours = GetNeighbours(worldCoord);

        // Lookup mesh name and rotation based on neighbours
        BlockInfo bi =
            SurfaceManager.GetSurface(
                SurfaceManager.GetID(neighbours));

        block.SetRotation(bi.rotation);

        for (int i=0; i<neighbours.Length; i++) {
            if (neighbours[i] == null) {
                continue;
            }
            UpdateNeighbour(neighbours[i], worldCoord + checkSides[i]);
        }

        PlaceBlock(msg.worldCoord, hash, bi);
    }

    private void UpdateNeighbours(Voxel[] neighbours, Point3 center) {
        for (int i=0; i<neighbours.Length; i++) {
            if (neighbours[i] == null) {
                continue;
            }
            UpdateNeighbour(neighbours[i], center + checkSides[i]);
        }
    }

    private void UpdateNeighbour(Voxel v, Point3 worldCoord) {
        BlockInfo bi =
            SurfaceManager.GetSurface(
                SurfaceManager.GetID(
                    GetNeighbours(worldCoord)));

        v.SetRotation(bi.rotation);

        int hash = ChunkManager.GetHash(worldCoord.ToChunkCoord());

        Transform chunkContainer = null;
        foreach (Transform child in transform) {
            if (child.name == hash.ToString()) {
                chunkContainer = child;
                break;
            }
        }

        if (chunkContainer == null) {
            PlaceBlock(worldCoord, hash, bi);
            return;
        }

        Mesh mesh = Resources.Load("Meshes/Dev/"+bi.meshName) as Mesh;
        string name = Name(worldCoord);

        foreach (Transform block in chunkContainer) {
            if (block.name == name) {
                block.GetComponent<MeshFilter>().sharedMesh = mesh;
                block.transform.localRotation = bi.rotation;
            }
        }
    }

    static public Voxel[] GetNeighbours(Point3 worldCoord) {
        Voxel[] neighbours = new Voxel[6];
        for (int i=0; i<checkSides.Length; i++) {
            neighbours[i] = ChunkManager.GetBlock(worldCoord + checkSides[i]);
        }
        return neighbours;
    }

    private void PlaceBlock(Point3 worldCoord, int hash, BlockInfo info) {
        Vector3 wCoord = worldCoord.ToVector3();
        wCoord.x += 0.5f;
        wCoord.y += 0.5f;
        wCoord.z += 0.5f;

        GameObject container = null;
        // Put code below in an event handler for OnFoundSurface??
        if (!chunks.ContainsKey(hash)) {
            container = GO.Create()
                .SetName(hash.ToString())
                .SetParent(transform)
                .gameObject;
            chunks.Add(hash, container);
        }
        chunks.TryGetValue(hash, out container);

        Mesh mesh = Resources.Load("Meshes/Dev/"+info.meshName) as Mesh;
        if (mesh == null) {
            mesh = voxelMesh;
        }

        GO.Create()
            .SetParent(container.transform)
            .SetMesh(mesh)
            .SetName(Name(worldCoord))
            .SetMaterial(voxelMat, true, ShadowCastingMode.On)
            .AddBoxCollider(Vector3.one * ChunkManager.blockSize)
            .SetPosition(wCoord)
            .SetRotation(info.rotation, true);
    }

    private string Name(Point3 coord) {
        return string.Format(objNameFormat, coord.x, coord.y, coord.z);
    }

    private string Name(Vector3 coord) {
        return string.Format(objNameFormat, coord.x, coord.y, coord.z);
    }

    private string Name(int x, int y, int z) {
        return string.Format(objNameFormat, x, y, z);
    }
}
