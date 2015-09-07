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
        Voxel block = chunkManager.GetBlock(msg.worldCoord);
        Point3 localCoord = chunkManager.GetLocalBlockCoord(msg.worldCoord);
        Point3 chunkCoord = chunkManager.GetChunkCoord(msg.worldCoord);
        int hash = chunkManager.GetHash(chunkCoord);
        GameObject container = null;

        if (block == null) {
            block = chunkManager.AddChunk(msg.worldCoord).Add(localCoord);
        }

        DataParser.SetBlockType(ref block.data, (int)msg.type);

        surfaceManager.FindSurface(ref block, msg.worldCoord);

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

    private void PlaceBlock(Vector3 worldCoord, Transform parent) {
        GOBuilder.Create()
            .SetParent(parent)
            .SetMesh(voxelMesh)
            .SetName(Name(worldCoord))
            .SetMaterial(voxelMat, false, ShadowCastingMode.Off)
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
