using UnityEngine;
using Mandarin;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VoxelSelector : MonoBehaviour {

    public void Create(float size) {
        Mesh mesh = new MeshBuilder()
            .CreateCube(size, size, size,
                        new Vector3(0.5f, 0.5f, 0.5f))
            .GetMesh();

        GetComponent<MeshFilter>().sharedMesh = mesh;

        Material mat = Resources.Load("VoxelSelector") as Material;
        GetComponent<MeshRenderer>().sharedMaterial = mat;
    }

}

