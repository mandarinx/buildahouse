using UnityEngine;
using UnityEngine.Rendering;
using Mandarin;
using HyperGames;

public class CoordDebugger : MonoBehaviour {

    public GameManager  gm;
    public Vector3      worldBlockCoord;
    public Vector3      localBlockCoord;
    public Vector3      chunkCoord;

    private Transform   selector;

    void Start() {
        GOBuilder.Create()
            .SetName("BlockManager")
            .AddComponent<BlockManager>((bm) => {
                bm.Init(gm.chunkManager);
            });

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
            PlacedBlock placedBlock = new PlacedBlock();
            placedBlock.type = BlockType.DIRT;
            placedBlock.worldCoord = bc;
            Messenger.Dispatch(placedBlock);
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
