using UnityEngine;
using System.Collections.Generic;

namespace Mandarin {
    public partial class MeshBuilder {

        public List<Vector3>    vertices { get; private set; }
        public List<Vector3>    normals { get; private set; }
        public List<Vector2>    uvs { get; private set; }

        private List<int>       indices;

        public MeshBuilder() {
            indices = new List<int>();
            vertices = new List<Vector3>();
            normals = new List<Vector3>();
            uvs = new List<Vector2>();
        }

        public void AddTriangle(int index0, int index1, int index2) {
            indices.Add(index0);
            indices.Add(index1);
            indices.Add(index2);
        }

        public Mesh CreateMesh() {
            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = indices.ToArray();

            // Normals are optional. Only use them if we have the correct amount:
            if (normals.Count == vertices.Count) {
                mesh.normals = normals.ToArray();
            }

            // UVs are optional. Only use them if we have the correct amount:
            if (uvs.Count == vertices.Count) {
                mesh.uv = uvs.ToArray();
            }

            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
