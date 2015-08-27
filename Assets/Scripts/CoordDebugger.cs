using UnityEngine;
using System.Collections;

public class CoordDebugger : MonoBehaviour {

    public GameManager gm;
    public Transform block;
    public Vector3 blockCoord;
    public Vector3 localBlockCoord;
    public Vector3 chunkCoord;

    void Start() {
        block.localScale = Vector3.one * gm.chunkManager.blockSize;
    }

    void Update () {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            Debug.DrawLine(ray.origin, hit.point, Color.blue);

            Point3 bc = gm.chunkManager.GetBlockCoord(hit.point);
            blockCoord.x = bc.x; blockCoord.y = bc.y; blockCoord.z = bc.z;

            Point3 lbc = gm.chunkManager.GetLocalBlockCoord(bc);
            localBlockCoord.x = lbc.x; localBlockCoord.y = lbc.y; localBlockCoord.z = lbc.z;

            Point3 cc = gm.chunkManager.GetChunkCoord(bc);
            chunkCoord.x = cc.x; chunkCoord.y = cc.y; chunkCoord.z = cc.z;

            block.position = (blockCoord * gm.chunkManager.blockSize) +
                             (Vector3.right * gm.chunkManager.blockSize * 0.5f) +
                             (Vector3.forward * gm.chunkManager.blockSize * 0.5f);
        }

        if (Input.GetMouseButtonUp(0)) {
            //  gm.chunkManager.
        }
    }
}
