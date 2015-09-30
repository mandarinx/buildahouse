using UnityEngine;
using HyperGames;

public class ScrollEvent : Message {
    public Vector2 direction;
}

public class PlacedBlock : Message {
    public Point3       worldCoord;
    public BlockType    type;
}

public class RemoveBlock : Message {
    public Point3       worldCoord;
}
