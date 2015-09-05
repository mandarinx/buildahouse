using UnityEngine;
using Mandarin;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VoxelSelector : MonoBehaviour {

    public void Create(float size) {
        MeshBuilder meshBuilder = new MeshBuilder();

        Vector3 upDir = Vector3.up * size;
        Vector3 rightDir = Vector3.right * size;
        Vector3 forwardDir = Vector3.forward * size;

        //  Vector3 nearCorner = new Vector3(-size * 0.5f, 0f, -size * 0.5f);
        Vector3 nearCorner = Vector3.zero;
        Vector3 farCorner = nearCorner + upDir + rightDir + forwardDir;

        meshBuilder.CreateQuad(nearCorner, forwardDir, rightDir);
        meshBuilder.CreateQuad(nearCorner, rightDir, upDir);
        meshBuilder.CreateQuad(nearCorner, upDir, forwardDir);

        meshBuilder.CreateQuad(farCorner, -rightDir, -forwardDir);
        meshBuilder.CreateQuad(farCorner, -upDir, -rightDir);
        meshBuilder.CreateQuad(farCorner, -forwardDir, -upDir);

        GetComponent<MeshFilter>().sharedMesh = meshBuilder.CreateMesh();

        Material mat = Resources.Load("VoxelSelector") as Material;
        GetComponent<MeshRenderer>().sharedMaterial = mat;
    }

}

