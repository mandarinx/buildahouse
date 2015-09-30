using System.Text;
using UnityEngine;

public class BlockDebugger {

    public GameManager      gameManager;
    public BlockManager     blockManager;
    public bool             enabled { get; private set; }
    public string           info {
        get {
            return sb.ToString();
        }
    }

    private StringBuilder   sb;
    public GUIStyle         style { get; private set; }

    public BlockDebugger() {
        style = new GUIStyle();
        style.normal.textColor = Color.black;
    }

    public void GetInfo(GameObject obj) {
        if (!CheckKey()) {
            return;
        }

        if (sb == null) {
            sb = new StringBuilder();
        }

        sb.Remove(0, sb.Length);

        Point3 worldCoord = Point3.ToWorldBlockCoord(obj.transform.position);
        Voxel block = ChunkManager.GetBlock(worldCoord);
        if (block == null) {
            return;
        }

        Voxel[] neighbours = BlockManager.GetNeighbours(worldCoord);
        int blockID = SurfaceManager.GetID(neighbours);
        Point3 chunkCoord = worldCoord.ToChunkCoord();
        int chunkID = ChunkManager.GetHash(chunkCoord);
        BlockInfo bi = SurfaceManager.GetSurface(blockID);

        sb.AppendLine("ID: "+blockID);
        sb.AppendLine("Type: "+block.GetBlockType());
        sb.AppendLine("Chunk ID: "+chunkID);
        sb.AppendLine("Mesh: "+bi.meshName);
        sb.AppendLine("Rotation: "+bi.rotation.eulerAngles.y+
                      "/"+obj.transform.rotation.eulerAngles.y+
                      "/"+block.GetRotation().eulerAngles.y);
        sb.AppendLine("Local coord: "+worldCoord.ToLocalBlockCoord());
        sb.AppendLine("World coord: "+worldCoord);
        sb.AppendLine("Chunk coord: "+chunkCoord);
    }

    private bool CheckKey() {
        enabled = Input.GetKey(KeyCode.LeftShift);
        return enabled;
    }
}
