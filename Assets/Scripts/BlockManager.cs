using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using Mandarin;
using HyperGames;

public class BlockManager : MonoBehaviour {

    private const string                    objNameFormat = "{0}_{1}_{2}";
    // Set to public due to BlockDebugger
    public ChunkManager                     chunkManager;
    private Material                        voxelMat;
    private Mesh                            voxelMesh;
    private Dictionary<int, GameObject>     chunks;
    public SurfaceManager                   surfaceManager;

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

        voxelMat = Resources.Load("Dev") as Material;

        Messenger.AddListener<PlacedBlock>(OnPlacedBlock);
        Messenger.AddListener<RemoveBlock>(OnRemoveBlock);
    }

    private void OnRemoveBlock(RemoveBlock msg) {
        Point3 worldCoord = msg.worldCoord;
        Chunk chunk = chunkManager.GetChunk(worldCoord);
        if (chunk == null) {
            return;
        }
        Point3 localCoord = chunkManager.GetLocalBlockCoord(worldCoord);
        chunk.RemoveBlock(localCoord);

        Point3 chunkCoord = chunkManager.GetChunkCoord(worldCoord);
        int hash = chunkManager.GetHash(chunkCoord);
        string name = Name(worldCoord);

        // TODO: Fix the deleting of gameobjects. I'd rather use a pool.
        GameObject container = chunks[hash];
        foreach (Transform child in container.transform) {
            if (child.name == name) {
                Destroy(child.gameObject);
            }
        }

        Voxel[] neighbours = GetNeighbours(worldCoord);
        for (int i=0; i<neighbours.Length; i++) {
            if (neighbours[i] == null) {
                continue;
            }

            if (i == 0) {
                UpdateNeighbour(ref neighbours[i].data, worldCoord.x, worldCoord.y, worldCoord.z + 1);
            }
            if (i == 1) {
                UpdateNeighbour(ref neighbours[i].data, worldCoord.x + 1, worldCoord.y, worldCoord.z);
            }
            if (i == 2) {
                UpdateNeighbour(ref neighbours[i].data, worldCoord.x, worldCoord.y, worldCoord.z - 1);
            }
            if (i == 3) {
                UpdateNeighbour(ref neighbours[i].data, worldCoord.x - 1, worldCoord.y, worldCoord.z);
            }
            if (i == 4) {
                UpdateNeighbour(ref neighbours[i].data, worldCoord.x, worldCoord.y + 1, worldCoord.z);
            }
            if (i == 5) {
                UpdateNeighbour(ref neighbours[i].data, worldCoord.x, worldCoord.y - 1, worldCoord.z);
            }
        }
    }

    private void OnPlacedBlock(PlacedBlock msg) {
        Point3 worldCoord = msg.worldCoord;
        Voxel block = chunkManager.GetBlock(worldCoord);
        if (block == null) {
            Point3 localCoord = chunkManager.GetLocalBlockCoord(worldCoord);
            block = chunkManager.AddChunk(worldCoord).AddBlock(localCoord);
        }

        Point3 chunkCoord = chunkManager.GetChunkCoord(worldCoord);
        int hash = chunkManager.GetHash(chunkCoord);
        DataParser.SetBlockType(ref block.data, (int)msg.type);

        // Get neighbours, pass to surface manager
        Voxel[] neighbours = GetNeighbours(worldCoord);

        // Lookup mesh name and rotation based on neighbours
        BlockInfo bi = surfaceManager.GetSurface(GetID(neighbours));
        DataParser.SetRotation(ref block.data, bi.rotation);

        for (int i=0; i<neighbours.Length; i++) {
            if (neighbours[i] == null) {
                continue;
            }

            if (i == 0) {
                UpdateNeighbour(ref neighbours[i].data, worldCoord.x, worldCoord.y, worldCoord.z + 1);
            }
            if (i == 1) {
                UpdateNeighbour(ref neighbours[i].data, worldCoord.x + 1, worldCoord.y, worldCoord.z);
            }
            if (i == 2) {
                UpdateNeighbour(ref neighbours[i].data, worldCoord.x, worldCoord.y, worldCoord.z - 1);
            }
            if (i == 3) {
                UpdateNeighbour(ref neighbours[i].data, worldCoord.x - 1, worldCoord.y, worldCoord.z);
            }
            if (i == 4) {
                UpdateNeighbour(ref neighbours[i].data, worldCoord.x, worldCoord.y + 1, worldCoord.z);
            }
            if (i == 5) {
                UpdateNeighbour(ref neighbours[i].data, worldCoord.x, worldCoord.y - 1, worldCoord.z);
            }
        }

        PlaceBlock(msg.worldCoord, hash, bi);
    }

    private void UpdateNeighbour(ref int data, int x, int y, int z) {
        Point3 worldCoord = new Point3(x, y, z);
        Voxel[] neighbours = GetNeighbours(worldCoord);
        BlockInfo bi = surfaceManager.GetSurface(GetID(neighbours));
        DataParser.SetRotation(ref data, bi.rotation);

        string name = Name(x, y, z);
        Point3 chunkCoord = chunkManager.GetChunkCoord(new Point3(x, y, z));
        int hash = chunkManager.GetHash(chunkCoord);

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

        foreach (Transform block in chunkContainer) {
            if (block.name == name) {
                block.GetComponent<MeshFilter>().sharedMesh = mesh;
                block.transform.localRotation = bi.rotation;
            }
        }
    }

    public Voxel[] GetNeighbours(Point3 worldCoord) {
        Voxel[] neighbours = new Voxel[6];
        int i = 0;

        // Sequence: z+ > x+ > z- > x- > y+ > y-
        neighbours[i++] = GetNeighbour(worldCoord,  0,  0,  1);
        neighbours[i++] = GetNeighbour(worldCoord,  1,  0,  0);
        neighbours[i++] = GetNeighbour(worldCoord,  0,  0, -1);
        neighbours[i++] = GetNeighbour(worldCoord, -1,  0,  0);
        neighbours[i++] = GetNeighbour(worldCoord,  0,  1,  0);
        neighbours[i++] = GetNeighbour(worldCoord,  0, -1,  0);

        return neighbours;
    }

    private Voxel GetNeighbour(Point3 worldCoord, int x, int y, int z) {
        return chunkManager.GetBlock(worldCoord.x + x, worldCoord.y + y, worldCoord.z + z);
    }

    // Requires neighbours to be listed in correct order.
    // That's bloody stupid!
    public int GetID(Voxel[] neighbours) {
        int id = 0;
        for (int i=0; i<neighbours.Length; i++) {

            if (neighbours[i] == null) {
                continue;
            }

            switch (i) {
                case 0: id += 1; break;
                case 1: id += 2; break;
                case 2: id += 4; break;
                case 3: id += 8; break;
                case 4: id += 16; break;
                case 5: id += 32; break;
            }
        }
        return id;
    }

    private void PlaceBlock(Point3 worldCoord, int hash, BlockInfo info) {
        Vector3 wCoord = worldCoord.ToVector3();
        wCoord.x += 0.5f;
        wCoord.y += 0.5f;
        wCoord.z += 0.5f;

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

        Mesh mesh = Resources.Load("Meshes/Dev/"+info.meshName) as Mesh;
        if (mesh == null) {
            mesh = voxelMesh;
        }

        GOBuilder.Create()
            .SetParent(container.transform)
            .SetMesh(mesh)
            .SetName(Name(worldCoord))
            .SetMaterial(voxelMat, true, ShadowCastingMode.On)
            .AddBoxCollider(Vector3.one * chunkManager.blockSize)
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
