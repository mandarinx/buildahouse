using UnityEngine;
using Mandarin;

public class Selection : MonoBehaviour {

    public LayerMask    mask;
    public Material     gridMat;
    public NineSlice    nineSlice;
    public float        unitSize = 1f;
    public float        gridSize = 10f;

    private Vector2     selectionPos;
    private Vector2     index;

    // 0: x offset
    // 1: y offset
    // 2: width
    // 3: height
    private float[]     selectionData;
    private GameObject  grid;
    private float       selX;
    private float       selZ;
    private float       selW;
    private float       selH;

    void Awake() {
        CreateGrid(gridSize, unitSize, ref grid);
        selectionData = new float[4]{0,0,0,0};
        nineSlice.SetSize(unitSize, unitSize);
    }

    void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, mask)) {
            index.x = Mathf.Round(hit.point.x / unitSize);
            index.y = Mathf.Round(hit.point.z / unitSize);
            selectionPos.x = index.x * unitSize;
            selectionPos.y = index.y * unitSize;

            nineSlice.SetPosition(selectionPos.x, 0f, selectionPos.y);
        } else {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            selectionData[0] = index.x;
            selectionData[1] = index.y;
            selectionData[2] = 1;
            selectionData[3] = 1;
            nineSlice.SetPosition(selectionPos.x, 0f, selectionPos.y);
            nineSlice.SetSize(unitSize, unitSize);
        }

        if (Input.GetMouseButton(0)) {
            selectionData[2] = index.x - selectionData[0];
            selectionData[3] = index.y - selectionData[1];

            selX = (selectionData[0] + (selectionData[2] / 2f)) * unitSize;
            selZ = (selectionData[1] + (selectionData[3] / 2f)) * unitSize;
            nineSlice.SetPosition(selX, 0f, selZ);

            selW = selectionData[2] >= 0 ? selectionData[2] + 1 : selectionData[2] - 1;
            selH = selectionData[3] >= 0 ? selectionData[3] + 1 : selectionData[3] - 1;
            nineSlice.SetSize(
                Mathf.Max(unitSize, Mathf.Abs(selW * unitSize)),
                Mathf.Max(unitSize, Mathf.Abs(selH * unitSize)));
        }
    }

    private void CreateGrid(float gridDim, float unitSize, ref GameObject grid) {
        grid = MeshUtils.CreatePlane(
            gridDim, gridDim, (int)(gridDim / unitSize), (int)(gridDim / unitSize),
            Vector3.zero, Orientation.Horizontal, 0f, 0f,
            false, true, "Grid");
        grid.layer = LayerMask.NameToLayer("Build");
        grid.GetComponent<MeshRenderer>().sharedMaterial = gridMat;
        grid.transform.position = new Vector3(0f, -0.001f, 0f);
        grid.GetComponent<BoxCollider>().center = new Vector3(0f, 0.001f, 0f);
    }

}
