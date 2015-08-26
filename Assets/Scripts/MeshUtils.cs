using UnityEngine;

namespace Mandarin {
    public enum Orientation {
        Horizontal = 0,
        Vertical = 1
    }
    
    public class MeshUtils {

        static public GameObject CreatePlane(float width,
                                            float length,
                                            int width_segments,
                                            int length_segments,
                                            Vector3 origin =           default(Vector3),
                                            Orientation orientation =  Orientation.Horizontal,
                                            float anchor_x =           0f,
                                            float anchor_y =           0f,
                                            bool two_sided =           false,
                                            bool add_collider =        false,
                                            string name =              "Plane") {
            GameObject plane = new GameObject();

            plane.name = name;
            plane.transform.position = origin;

            Mathf.Clamp(anchor_x, -0.5f, 0.5f);
            Mathf.Clamp(anchor_y, -0.5f, 0.5f);

            Vector2 anchorOffset = new Vector2(width * anchor_x, length * anchor_y);

            MeshFilter meshFilter = plane.AddComponent<MeshFilter>();
            plane.AddComponent<MeshRenderer>();

            Mesh m = new Mesh();
                m.name = plane.name;

                int hCount2 = width_segments + 1;
                int vCount2 = length_segments + 1;
                int numTriangles = width_segments * length_segments * 6;
                if (two_sided) {
                    numTriangles *= 2;
                }
                int numVertices = hCount2 * vCount2;

                Vector3[] vertices = new Vector3[numVertices];
                Vector2[] uvs = new Vector2[numVertices];
                int[] triangles = new int[numTriangles];

                int index = 0;
                float uvFactorX = 1.0f / width_segments;
                float uvFactorY = 1.0f / length_segments;
                float scaleX = width / width_segments;
                float scaleY = length / length_segments;
                for (float y = 0.0f; y < vCount2; y++) {
                    for (float x = 0.0f; x < hCount2; x++) {
                        if (orientation == Orientation.Horizontal) {
                            vertices[index] = new Vector3(
                                x * scaleX - width/2f - anchorOffset.x,
                                0.0f,
                                y * scaleY - length/2f - anchorOffset.y);
                        } else {
                            vertices[index] = new Vector3(
                                x * scaleX - width/2f - anchorOffset.x,
                                y * scaleY - length/2f - anchorOffset.y,
                                0.0f);
                        }
                        uvs[index++] = new Vector2(x*uvFactorX, y*uvFactorY);
                    }
                }

                index = 0;
                for (int y = 0; y < length_segments; y++) {
                    for (int x = 0; x < width_segments; x++) {
                        triangles[index]   = (y     * hCount2) + x;
                        triangles[index+1] = ((y+1) * hCount2) + x;
                        triangles[index+2] = (y     * hCount2) + x + 1;

                        triangles[index+3] = ((y+1) * hCount2) + x;
                        triangles[index+4] = ((y+1) * hCount2) + x + 1;
                        triangles[index+5] = (y     * hCount2) + x + 1;
                        index += 6;
                    }
                    if (two_sided) {
                        // Same tri vertices with order reversed, so normals point in the opposite direction
                        for (int x = 0; x < width_segments; x++) {
                            triangles[index]   = (y     * hCount2) + x;
                            triangles[index+1] = (y     * hCount2) + x + 1;
                            triangles[index+2] = ((y+1) * hCount2) + x;

                            triangles[index+3] = ((y+1) * hCount2) + x;
                            triangles[index+4] = (y     * hCount2) + x + 1;
                            triangles[index+5] = ((y+1) * hCount2) + x + 1;
                            index += 6;
                        }
                    }
                }

                m.vertices = vertices;
                m.uv = uvs;
                m.triangles = triangles;
                m.RecalculateNormals();

            meshFilter.sharedMesh = m;
            m.RecalculateBounds();

            if (add_collider) {
                plane.AddComponent<BoxCollider>();
            }

            return plane;
        }
    }
}
