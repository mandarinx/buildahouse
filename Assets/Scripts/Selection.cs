using UnityEngine;
using Mandarin;

[RequireComponent(typeof(NineSlice))]
public class Selection : MonoBehaviour {

    // TODO:
    // Selection grid must match voxel grid. Take blockSize intro account.
    
    public LayerMask    mask;
    public float        unitSize = 1f;
    public float        gridSize = 10f;
    public bool         lockOnRelease = true;

    private NineSlice   nineSlice;
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
        grid = MeshUtils.CreatePlane(50f, 50f, 1, 1, new Vector3(0f, -0.001f, 0f));
        grid.layer = LayerMask.NameToLayer("Selection");
        grid.AddComponent<BoxCollider>().center = new Vector3(0f, 0.001f, 0f);
        grid.name = "SelectionBase";
        Destroy(grid.GetComponent<MeshRenderer>());

        selectionData = new float[4]{0,0,0,0};

        nineSlice = GetComponent<NineSlice>();
        nineSlice.SetSize(unitSize, unitSize);
        nineSlice.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, mask)) {
            index.x = Mathf.Round(hit.point.x / unitSize);
            index.y = Mathf.Round(hit.point.z / unitSize);
            selectionPos.x = index.x * unitSize;
            selectionPos.y = index.y * unitSize;
        } else {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            selectionData[0] = index.x;
            selectionData[1] = index.y;
            selectionData[2] = 1;
            selectionData[3] = 1;

            nineSlice.SetPosition(
                selectionPos.x,
                hit.point.y + 0.01f,
                selectionPos.y);
            nineSlice.SetSize(unitSize, unitSize);
        }

        if (Input.GetMouseButton(0)) {
            selectionData[2] = index.x - selectionData[0];
            selectionData[3] = index.y - selectionData[1];

            selX = (selectionData[0] + (selectionData[2] / 2f)) * unitSize;
            selZ = (selectionData[1] + (selectionData[3] / 2f)) * unitSize;
            nineSlice.SetPosition(selX, hit.point.y + 0.01f, selZ);

            selW = selectionData[2] >= 0 ? selectionData[2] + 1 : selectionData[2] - 1;
            selH = selectionData[3] >= 0 ? selectionData[3] + 1 : selectionData[3] - 1;
            nineSlice.SetSize(
                Mathf.Abs(selW * unitSize),
                Mathf.Abs(selH * unitSize));
        }

        if (Input.GetMouseButtonUp(0)) {
            Callbacks.SendSelectionData(selectionData);
        }
    }

}
