using UnityEngine;
using Mandarin;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VoxelSelector : MonoBehaviour {

    private Mesh cube;
    private Mesh plane;

    public void Create(float size) {
        cube = new MeshBuilder()
            .CreateCube(size, size, size,
                        new Vector3(0.5f, 0.5f, 0.5f))
            .GetMesh();

        plane = new MeshBuilder()
            .CreateQuad(new Vector3(-0.5f, 0f, 0.5f),
                        -Vector3.forward,
                        Vector3.right)
            .GetMesh();

        GetComponent<MeshFilter>().sharedMesh = plane;

        Material mat = Resources.Load("VoxelSelector") as Material;
        GetComponent<MeshRenderer>().sharedMaterial = mat;
    }

    public void UseCube() {
        GetComponent<MeshFilter>().sharedMesh = cube;
    }

    public void UsePlane() {
        GetComponent<MeshFilter>().sharedMesh = plane;
    }
}

