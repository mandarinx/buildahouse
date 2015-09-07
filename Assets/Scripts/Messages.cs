using UnityEngine;
using HyperGames;

public class ScrollEvent : Message {
    public Vector2 direction;
    public ScrollEvent(Vector2 direction) {
        this.direction = direction;
    }
}

public class PlacedBlock : Message {
    public Point3       worldCoord;
    public BlockType    type;
}
