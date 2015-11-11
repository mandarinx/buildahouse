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
        Vector3 pos = Vector3.zero;

        if (Physics.Raycast(ray, out hit)) {
            blockDebugger.GetInfo(hit.transform.gameObject);
            Debug.DrawLine(ray.origin, hit.point, Color.magenta);
            Debug.DrawLine(hit.point, hit.point + (hit.normal * 0.5f), Color.cyan);

            bc = Point3.ToWorldBlockCoord(hit.point);
            worldBlockCoord = bc.ToVector3();
            lbc = bc.ToLocalBlockCoord();
            localBlockCoord = lbc.ToVector3();
            Point3 cc = bc.ToChunkCoord();
            chunkCoord = cc.ToVector3();

            if (Input.GetKey(KeyCode.LeftControl)) {
                selector.UseCube();
                pos = hit.point - hit.normal * 0.5f;
            } else {
                selector.UsePlane();
                selector.transform.up = hit.normal;
                pos = hit.point;
            }

            // TODO: Look into using cross product to get rid of branching

            if (hit.normal == Vector3.right ||
                hit.normal == -Vector3.right) {
                pos.y = worldBlockCoord.y + 0.5f;
                pos.z = worldBlockCoord.z + 0.5f;
            }

            if (hit.normal == Vector3.forward ||
                hit.normal == -Vector3.forward) {
                pos.x = worldBlockCoord.x + 0.5f;
                pos.y = worldBlockCoord.y + 0.5f;
            }

            if (hit.normal == Vector3.up ||
                hit.normal == -Vector3.up) {
                pos.x = worldBlockCoord.x + 0.5f;
                pos.z = worldBlockCoord.z + 0.5f;
            }

            selectedBlock = Point3.ToWorldBlockCoord(pos + hit.normal * 0.5f);
            selector.transform.position = pos + hit.normal * 0.0001f;
        }

        if (Input.GetMouseButtonUp(0)) {
            if (Input.GetKey(KeyCode.LeftControl)) {
                Messenger.Dispatch(new RemoveBlock() {
                    worldCoord = Point3.ToWorldBlockCoord(pos)
                });
                return;
            }

            Messenger.Dispatch(new PlacedBlock() {
                type = BlockType.DIRT,
                worldCoord = selectedBlock
            });
        }
    }

    void OnGUI() {
        if (blockDebugger.enabled) {
            GUI.Label(new Rect(0f, 0f, 300f, 300f), blockDebugger.info, blockDebugger.style);
        }
    }

    private void AddVoxelSelector() {
        selector = GO.Create()
            .SetName("VoxelSelector")
            .SetParent(transform)
            .AddComponent<VoxelSelector>((vs) => {
                vs.Create(ChunkManager.blockSize);
            })
            .gameObject
            .GetComponent<VoxelSelector>();
    }
}
