using UnityEngine;
using Mandarin;
using HyperGames;

// Rewrite this class to be some input manager kind of type

public class CoordDebugger : MonoBehaviour {

    public GameManager      gm;
    public Vector3          worldBlockCoord;
    public Vector3          localBlockCoord;
    public Vector3          chunkCoord;

    private VoxelSelector   selector;
    private BlockDebugger   blockDebugger;
    private Point3          selectedBlock;

    void Start() {
        blockDebugger = new BlockDebugger();
        blockDebugger.gameManager = gm;
        blockDebugger.blockManager = gm.blockManager;

        AddVoxelSelector();
    }

    void Update () {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Point3 bc = Point3.zero;
        Point3 lbc = Point3.zero;

        if (Physics.Raycast(ray, out hit)) {
            blockDebugger.GetInfo(hit.transform.gameObject);
            Debug.DrawLine(ray.origin, hit.point, Color.magenta);
            Debug.DrawLine(hit.point, hit.point + (hit.normal * 0.5f), Color.cyan);

            Vector3 coord = hit.point;

            bc = gm.chunkManager.GetWorldBlockCoord(coord);
            bc.CopyTo(ref worldBlockCoord);

            lbc = gm.chunkManager.GetLocalBlockCoord(bc);
            lbc.CopyTo(ref localBlockCoord);

            Point3 cc = gm.chunkManager.GetChunkCoord(bc);
            cc.CopyTo(ref chunkCoord);

            Vector3 p = Vector3.zero;
            if (Input.GetKey(KeyCode.LeftControl)) {
                selector.UseCube();
                p = hit.point - hit.normal * 0.5f;
            } else {
                selector.UsePlane();
                selector.transform.up = hit.normal;
                p = hit.point;
            }

            if (hit.normal == Vector3.right ||
                hit.normal == -Vector3.right) {
                p.y = worldBlockCoord.y + 0.5f;
                p.z = worldBlockCoord.z + 0.5f;
            }

            if (hit.normal == Vector3.forward ||
                hit.normal == -Vector3.forward) {
                p.x = worldBlockCoord.x + 0.5f;
                p.y = worldBlockCoord.y + 0.5f;
            }

            if (hit.normal == Vector3.up ||
                hit.normal == -Vector3.up) {
                p.x = worldBlockCoord.x + 0.5f;
                p.z = worldBlockCoord.z + 0.5f;
            }

            selectedBlock = gm.chunkManager.GetWorldBlockCoord(p + hit.normal * 0.5f);

            selector.transform.position = p;
        }

        if (Input.GetMouseButtonUp(0)) {
            if (Input.GetKey(KeyCode.LeftControl)) {
                RemoveBlock rmBlock = new RemoveBlock();
                rmBlock.worldCoord = gm.chunkManager.GetWorldBlockCoord(selector.transform.position);
                Messenger.Dispatch(rmBlock);
                return;
            }

            PlacedBlock placedBlock = new PlacedBlock();
            placedBlock.type = BlockType.DIRT;
            placedBlock.worldCoord = selectedBlock;
            Messenger.Dispatch(placedBlock);
        }
    }

    void OnGUI() {
        if (blockDebugger.enabled) {
            GUI.Label(new Rect(0f, 0f, 300f, 300f), blockDebugger.info, blockDebugger.style);
        }
    }

    private void AddVoxelSelector() {
        selector = GOBuilder.Create()
            .SetName("VoxelSelector")
            .SetParent(transform)
            .AddComponent<VoxelSelector>((vs) => {
                vs.Create(gm.chunkManager.blockSize);
            })
            .GameObject.GetComponent<VoxelSelector>();
    }
}
