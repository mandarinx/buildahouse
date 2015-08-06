using UnityEngine;
using System.Collections.Generic;

public class Highlighter : MonoBehaviour {

    public Transform marker;
    public LayerMask mask;
    public Vector3 pos;
    private float gridSize = 1f;
    private GameObject prefab;
    private List<GameObject> cubes;

    void Awake() {
        prefab = Resources.Load("Cube") as GameObject;
        cubes = new List<GameObject>(20);
        Create(Vector3.zero);
    }

    private void Create(Vector3 pos) {
        cubes.Add(GameObject.Instantiate(prefab, pos, Quaternion.identity) as GameObject);
    }

    private float Snap(float value) {
        //  return Mathf.Floor(Mathf.Abs(value) / gridSize) * Mathf.Sign(value);
        return Mathf.Floor(Mathf.Abs(value + (gridSize / 2f)) / gridSize) * Mathf.Sign(value);
    }

    void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, mask)) {
            marker.up = hit.normal;
            pos = hit.point;

            pos.x = Snap(pos.x);
            pos.y = Snap(pos.y);
            pos.z = Snap(pos.z);
            //  pos.x = Mathf.Floor(pos.x / gridSize);
            //  pos.y = Mathf.Floor(pos.y / gridSize);
            //  pos.z = Mathf.Floor(pos.z / gridSize);

            //  if (hit.normal == Vector3.up) {
            //      pos.x = Mathf.Round(pos.x / gridSize);
            //      pos.y = Mathf.Round(pos.y / gridSize) + 0.5f;
            //      pos.z = Mathf.Round(pos.z / gridSize);
            //  }
            //  if (hit.normal == -Vector3.up) {
            //      pos.x = Mathf.Round(pos.x / gridSize);
            //      pos.y = Mathf.Round(pos.y / gridSize) - 0.5f;
            //      pos.z = Mathf.Round(pos.z / gridSize);
            //  }
            //  if (hit.normal == Vector3.forward) {
            //      pos.x = Mathf.Round(pos.x / gridSize);
            //      pos.y = Mathf.Round(pos.y / gridSize);
            //      pos.z = Mathf.Round(pos.z / gridSize) + 0.5f;
            //  }
            //  if (hit.normal == -Vector3.forward) {
            //      pos.x = Mathf.Round(pos.x / gridSize);
            //      pos.y = Mathf.Round(pos.y / gridSize);
            //      pos.z = Mathf.Round(pos.z / gridSize) - 0.5f;
            //  }
            //  if (hit.normal == Vector3.right) {
            //      pos.x = Mathf.Round(pos.x / gridSize) + 0.5f;
            //      pos.y = Mathf.Round(pos.y / gridSize);
            //      pos.z = Mathf.Round(pos.z / gridSize);
            //  }
            //  if (hit.normal == -Vector3.right) {
            //      pos.x = Mathf.Round(pos.x / gridSize) - 0.5f;
            //      pos.y = Mathf.Round(pos.y / gridSize);
            //      pos.z = Mathf.Round(pos.z / gridSize);
            //  }

            marker.position = pos;
        }

        if (Input.GetMouseButtonUp(0)) {
            Create(pos + hit.normal);
        }
    }
}
