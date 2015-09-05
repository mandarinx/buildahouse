using UnityEngine;
using UnityEngine.Rendering;
using Mandarin;

public class CoordDebugger : MonoBehaviour {

    public GameManager  gm;
    public Vector3      worldBlockCoord;
    public Vector3      localBlockCoord;
    public Vector3      chunkCoord;

    private Transform   selector;
    private Transform   parent;
    private Material    voxelMat;
    private Mesh        voxelMesh;

    void Start() {
        parent = GOBuilder.Create()
                    .SetName("Voxels")
                    .GameObject.transform;

        voxelMesh = new MeshBuilder()
            .CreateCube(gm.chunkManager.blockSize,
                        gm.chunkManager.blockSize,
                        gm.chunkManager.blockSize,
                        new Vector3(0.5f, 0.5f, 0.5f))
            .GetMesh();

        voxelMat = Resources.Load("Voxel") as Material;
        AddVoxelSelector();
    }

    void Update () {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Point3 bc = Point3.zero;
        Point3 lbc = Point3.zero;

        if (Physics.Raycast(ray, out hit)) {
            Debug.DrawLine(ray.origin, hit.point, Color.magenta);
            Debug.DrawLine(hit.point, hit.point + (hit.normal * 0.5f), Color.cyan);

            Vector3 coord = hit.point + (hit.normal * 0.5f);

            bc = gm.chunkManager.GetWorldBlockCoord(coord);
            bc.CopyTo(ref worldBlockCoord);

            lbc = gm.chunkManager.GetLocalBlockCoord(bc);
            lbc.CopyTo(ref localBlockCoord);

            Point3 cc = gm.chunkManager.GetChunkCoord(bc);
            cc.CopyTo(ref chunkCoord);

            selector.position = new Vector3(worldBlockCoord.x + 0.5f,
                                            worldBlockCoord.y + 0.5f,
                                            worldBlockCoord.z + 0.5f);
        }

        if (Input.GetMouseButtonUp(0)) {
            Voxel block = gm.chunkManager.GetBlock(bc);
            if (block == null) {
                block = gm.chunkManager.AddChunk(bc).Add(lbc);
            }
            DataParser.SetBlockType(ref block.data, 1);

            Vector3 bcv3 = bc.ToVector3();
            bcv3.x += 0.5f;
            bcv3.y += 0.5f;
            bcv3.z += 0.5f;
            PlaceBlock(bcv3);
        }
    }

    private void PlaceBlock(Vector3 worldCoord) {
        GOBuilder.Create()
            .SetParent(parent)
            .SetMesh(voxelMesh)
            .SetMaterial(voxelMat, false, ShadowCastingMode.Off)
            .AddBoxCollider(Vector3.one * gm.chunkManager.blockSize)
            .SetPosition(worldCoord);
    }

    private void AddVoxelSelector() {
        selector = GOBuilder.Create()
            .SetName("VoxelSelector")
            .SetParent(transform)
            .AddComponent<VoxelSelector>((vs) => {
                vs.Create(gm.chunkManager.blockSize);
            })
            .GameObject.transform;
    }
}
