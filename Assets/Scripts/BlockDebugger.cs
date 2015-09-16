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

        ChunkManager cm = blockManager.chunkManager;

        if (sb == null) {
            sb = new StringBuilder();
        }

        sb.Remove(0, sb.Length);

        Point3 worldCoord = cm.GetWorldBlockCoord(obj.transform.position);
        Voxel block = cm.GetBlock(worldCoord);
        if (block == null) {
            return;
        }
        
        Point3 localCoord = cm.GetLocalBlockCoord(worldCoord);
        Voxel[] neighbours = blockManager.GetNeighbours(worldCoord);
        int blockID = blockManager.GetID(neighbours);
        Point3 chunkCoord = cm.GetChunkCoord(worldCoord);
        int chunkID = cm.GetHash(chunkCoord);
        BlockInfo bi = blockManager.surfaceManager.GetSurface(blockID);
        BlockType type = (BlockType)DataParser.GetBlockType(block.data);

        sb.AppendLine("ID: "+blockID);
        sb.AppendLine("Type: "+type);
        sb.AppendLine("Chunk ID: "+chunkID);
        sb.AppendLine("Mesh: "+bi.meshName);
        sb.AppendLine("Rotation: "+bi.rotation.eulerAngles.y+
                      "/"+obj.transform.rotation.eulerAngles.y+
                      "/"+DataParser.GetRotation(block.data).eulerAngles.y);
        sb.AppendLine("Local coord: "+localCoord);
        sb.AppendLine("World coord: "+worldCoord);
        sb.AppendLine("Chunk coord: "+chunkCoord);
    }

    private bool CheckKey() {
        enabled = Input.GetKey(KeyCode.LeftShift);
        return enabled;
    }
}
