using UnityEngine;

namespace Mandarin {
    public partial class MeshBuilder {

        public MeshBuilder CreateQuad(Vector3       offset,
                                      Vector3       widthDir,
                                      Vector3       lengthDir) {

            Vector3 normal = Vector3.Cross(lengthDir, widthDir).normalized;

            vertices.Add(offset);
            uvs.Add(new Vector2(0.0f, 0.0f));
            normals.Add(normal);

            vertices.Add(offset + lengthDir);
            uvs.Add(new Vector2(0.0f, 1.0f));
            normals.Add(normal);

            vertices.Add(offset + lengthDir + widthDir);
            uvs.Add(new Vector2(1.0f, 1.0f));
            normals.Add(normal);

            vertices.Add(offset + widthDir);
            uvs.Add(new Vector2(1.0f, 0.0f));
            normals.Add(normal);

            int baseIndex = vertices.Count - 4;

            AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
            AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);

            return this;
        }

    }
}
