using UnityEngine;
using System.Collections.Generic;

public class TileIndexEntry {
    public string[] index;
    // Corners:     1100, 0110, 0011, 1001
    // Straights:   1010, 0101
    // Filler:      1111
}

public class TileIndex {

    private List<TileIndexEntry> entries;

    public TileIndex() {
        entries = new List<TileIndexEntry>();

        entries.Add(new TileIndexEntry {
            index = new string[4]{"1100", "0110", "0011", "1001"}
        });
    }

    private float GetRotation(string index) {
        switch (index) {
            case "0011":
            case "0101":
            case "0111":
                return 0f;
            case "1001":
            case "1011":
                return 90f;
            case "1100":
            case "1101":
                return 180f;
            case "0110":
            case "1110":
                return 270f;
        }
        return 0f;
    }
}
