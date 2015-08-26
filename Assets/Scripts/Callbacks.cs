using UnityEngine;
using System.Collections;

public delegate void SelectionDataDelegate(float[] data);

static public class Callbacks {

    static public SelectionDataDelegate     OnSelectionData;

    static public void SendSelectionData(float[] data) {
        if (OnSelectionData != null) {
            OnSelectionData(data);
        }
    }
}
