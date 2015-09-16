using UnityEngine;
using Mandarin;
using HyperGames;

public class ThemeTester : MonoBehaviour {

    public string           theme;
    public GameManager      gameManager;

    private ChunkManager    cm;
    private PlacedBlock     placedBlock;

    void Start() {
        cm = gameManager.chunkManager;
        placedBlock = new PlacedBlock();

        // 0
        Place(0, 0, 0);

        // 1, 2, 4, 8
        Place(2, 0, 0);
        Place(3, 0, 0);
        Place(5, 0, 0);
        Place(5, 0, 1);

        // 16, 32
        Place(0, 1, 2);
        Place(0, 2, 2);

        // 3, 6, 12, 9
        Place(7, 0, 0);
        Place(8, 0, 0);
        Place(7, 0, 1);

        Place(9, 0, 1);
        Place(10, 0, 1);
        Place(10, 0, 0);

        Place(7, 0, 3);
        Place(7, 0, 4);
        Place(8, 0, 4);

        Place(9, 0, 3);
        Place(10, 0, 3);
        Place(10, 0, 4);

        // 17, 18, 20, 24
    }

    private void Place(int x, int y, int z) {
        Point3 worldCoord = new Point3(x, y, z);
        Point3 localCoord = cm.GetLocalBlockCoord(worldCoord);
        cm.AddChunk(worldCoord).AddBlock(localCoord);

        placedBlock.type = BlockType.DIRT;
        placedBlock.worldCoord = worldCoord;
        Messenger.Dispatch(placedBlock);
    }
}
