using UnityEngine;
using UnityEngine.Rendering;
using Mandarin;

public class CoordDebugger : MonoBehaviour {

    public GameManager  gm;
    public Vector3      worldBlockCoord;
    public Vector3      localBlockCoord;
    public Vector3      chunkCoord;

    private Transform   selector;

    void Start() {
        //  selector.localScale = Vector3.one * gm.chunkManager.blockSize;
        AddVoxelSelector();
    }

    void Update () {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Point3 bc = Point3.zero;
        Point3 lbc = Point3.zero;

        if (Physics.Raycast(ray, out hit)) {
            Debug.DrawLine(ray.origin, hit.point, Color.blue);

            bc = gm.chunkManager.GetWorldBlockCoord(hit.point);
            bc.CopyTo(ref worldBlockCoord);

            lbc = gm.chunkManager.GetLocalBlockCoord(bc);
            lbc.CopyTo(ref localBlockCoord);

            Point3 cc = gm.chunkManager.GetChunkCoord(bc);
            cc.CopyTo(ref chunkCoord);

            // Works because pivot of the selector is in the lower left corner of the cube
            selector.position = worldBlockCoord;
        }

        if (Input.GetMouseButtonUp(0)) {
            Voxel block = gm.chunkManager.GetBlock(bc);
            if (block == null) {
                block = gm.chunkManager.AddChunk(bc).Add(lbc);
            }
            DataParser.SetBlockType(ref block.data, 1);
        }
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
