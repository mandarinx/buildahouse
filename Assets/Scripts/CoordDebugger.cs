using UnityEngine;
using Mandarin;
using HyperGames;

// This class seems to have mutated into an input-game-manager-hybrid

public class CoordDebugger : MonoBehaviour {

    public GameManager      gm;
    public Vector3          worldBlockCoord;
    public Vector3          localBlockCoord;
    public Vector3          chunkCoord;

    private Transform       selector;
    private BlockDebugger   blockDebugger;

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

            //  Vector3 coord = hit.point + (hit.normal * 0.5f);
            Vector3 coord = hit.point;

            bc = gm.chunkManager.GetWorldBlockCoord(coord);
            bc.CopyTo(ref worldBlockCoord);

            lbc = gm.chunkManager.GetLocalBlockCoord(bc);
            lbc.CopyTo(ref localBlockCoord);

            Point3 cc = gm.chunkManager.GetChunkCoord(bc);
            cc.CopyTo(ref chunkCoord);

            selector.position = new Vector3(worldBlockCoord.x + 0.5f,
                                            worldBlockCoord.y + 0.5f,
                                            worldBlockCoord.z + 0.5f);
            //  selector.position = hit.transform.position;
        }

        if (Input.GetMouseButtonUp(0)) {
            if (Input.GetKey(KeyCode.LeftControl)) {
                RemoveBlock rmBlock = new RemoveBlock();
                rmBlock.worldCoord = bc;
                Messenger.Dispatch(rmBlock);
                return;
            }

            PlacedBlock placedBlock = new PlacedBlock();
            placedBlock.type = BlockType.DIRT;
            placedBlock.worldCoord = bc;
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
            .GameObject.transform;
    }
}
