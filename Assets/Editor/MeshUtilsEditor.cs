using UnityEditor;
using UnityEngine;

namespace Statnett.Utils {
    public class MeshUtilsEditor {

        static public void DuplicateMesh(Mesh original, string path) {
            Mesh newmesh = new Mesh();
            newmesh.vertices = original.vertices;
            newmesh.triangles = original.triangles;
            newmesh.uv = original.uv;
            newmesh.normals = original.normals;

            newmesh.colors = new Color[original.colors.Length];
            for (int i=0; i<original.colors.Length; i++) {
                newmesh.colors[i] = original.colors[i];
            }

            newmesh.tangents = original.tangents;
            AssetDatabase.CreateAsset(newmesh, path + ".asset");
        }
    }
}
