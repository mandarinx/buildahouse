using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using Mandarin;
using HyperGames;

public class BlockManager : MonoBehaviour {

    private const string                    objNameFormat = "{0}_{1}_{2}";
    private ChunkManager                    chunkManager;
    private Material                        voxelMat;
    private Mesh                            voxelMesh;
    private Dictionary<int, GameObject>     chunks;
    private SurfaceManager                  surfaceManager;

    public void Init(ChunkManager cm) {
        chunkManager = cm;
        chunks = new Dictionary<int, GameObject>();
        surfaceManager = new SurfaceManager(cm);

        voxelMesh = new MeshBuilder()
            .CreateCube(chunkManager.blockSize,
                        chunkManager.blockSize,
                        chunkManager.blockSize,
                        new Vector3(0.5f, 0.5f, 0.5f))
            .GetMesh();

        voxelMat = Resources.Load("Voxel") as Material;

        Messenger.AddListener<PlacedBlock>(OnPlacedBlock);
    }

    private void OnPlacedBlock(PlacedBlock msg) {
        Point3 worldCoord = msg.worldCoord;
        Voxel block = chunkManager.GetBlock(worldCoord);
        if (block == null) {
            Point3 localCoord = chunkManager.GetLocalBlockCoord(worldCoord);
            block = chunkManager.AddChunk(worldCoord).Add(localCoord);
        }

        Point3 chunkCoord = chunkManager.GetChunkCoord(worldCoord);
        int hash = chunkManager.GetHash(chunkCoord);
        DataParser.SetBlockType(ref block.data, (int)msg.type);

        // Get neighbours, pass to surface manager
        Voxel[] neighbours = GetNeighbours(worldCoord);

        // Lookup mesh name and rotation based on neighbours
        BlockInfo bi = surfaceManager.GetSurface(neighbours);
        DataParser.SetRotation(ref block.data, bi.rotation);

        for (int i=0; i<neighbours.Length; i++) {
            if (neighbours[i] == null) {
                continue;
            }

            if (i == 0) {
                UpdateNeighbour(ref neighbours[i].data, worldCoord.x + 1, worldCoord.y, worldCoord.z);
            }
            if (i == 1) {
                UpdateNeighbour(ref neighbours[i].data, worldCoord.x - 1, worldCoord.y, worldCoord.z);
            }
            if (i == 2) {
                UpdateNeighbour(ref neighbours[i].data, worldCoord.x, worldCoord.y + 1, worldCoord.z);
            }
            if (i == 3) {
                UpdateNeighbour(ref neighbours[i].data, worldCoord.x, worldCoord.y - 1, worldCoord.z);
            }
            if (i == 4) {
                UpdateNeighbour(ref neighbours[i].data, worldCoord.x, worldCoord.y, worldCoord.z + 1);
            }
            if (i == 5) {
                UpdateNeighbour(ref neighbours[i].data, worldCoord.x, worldCoord.y, worldCoord.z - 1);
            }
        }

        GameObject container = null;
        // Put code below in an event handler for OnFoundSurface??
        if (!chunks.ContainsKey(hash)) {
            container = GOBuilder.Create()
                .SetName(hash.ToString())
                .SetParent(transform)
                .GameObject;
            chunks.Add(hash, container);
        }
        chunks.TryGetValue(hash, out container);

        Vector3 bcv3 = msg.worldCoord.ToVector3();
        bcv3.x += 0.5f;
        bcv3.y += 0.5f;
        bcv3.z += 0.5f;
        PlaceBlock(bcv3, container.transform);
    }

    private void UpdateNeighbour(ref int data, int x, int y, int z) {
        Voxel[] neighbours = GetNeighbours(new Point3(x, y, z));
        BlockInfo bi = surfaceManager.GetSurface(neighbours);
        DataParser.SetRotation(ref data, bi.rotation);

        // TODO: Update GameObject
    }

    private Voxel[] GetNeighbours(Point3 coord) {
        Voxel[] neighbours = new Voxel[6];
        int i = 0;

        // Sequence: z+ > x+ > z- > x- > y+ > y-
        neighbours[i++] = GetNeighbour(coord,  0,  0,  1);
        neighbours[i++] = GetNeighbour(coord,  1,  0,  0);
        neighbours[i++] = GetNeighbour(coord,  0,  0, -1);
        neighbours[i++] = GetNeighbour(coord, -1,  0,  0);
        neighbours[i++] = GetNeighbour(coord,  0,  1,  0);
        neighbours[i++] = GetNeighbour(coord,  0, -1,  0);

        return neighbours;
    }

    private Voxel GetNeighbour(Point3 worldCoord, int x, int y, int z) {
        return chunkManager.GetBlock(worldCoord.x + x, worldCoord.y + y, worldCoord.z + z);
    }

    private void PlaceBlock(Vector3 worldCoord, Transform parent) {
        GOBuilder.Create()
            .SetParent(parent)
            .SetMesh(voxelMesh)
            .SetName(Name(worldCoord))
            .SetMaterial(voxelMat, true, ShadowCastingMode.On)
            .AddBoxCollider(Vector3.one * chunkManager.blockSize)
            .SetPosition(worldCoord);
    }

    private string Name(Point3 coord) {
        return string.Format(objNameFormat, coord.x, coord.y, coord.z);
    }

    private string Name(Vector3 coord) {
        return string.Format(objNameFormat, coord.x, coord.y, coord.z);
    }
}
