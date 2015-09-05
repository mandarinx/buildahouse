using UnityEngine;

namespace Mandarin {
    public partial class MeshBuilder {

        public MeshBuilder CreateCube(float width, float length, float height,
                                      Vector3 pivot = default(Vector3)) {

            Vector3 upDir = Vector3.up * height;
            Vector3 rightDir = Vector3.right * width;
            Vector3 forwardDir = Vector3.forward * length;

            Vector3 nearCorner = new Vector3(width * -pivot.x,
                                             height * -pivot.y,
                                             length * -pivot.z);
            Vector3 farCorner = nearCorner + upDir + rightDir + forwardDir;

            CreateQuad(nearCorner, forwardDir, rightDir);
            CreateQuad(nearCorner, rightDir, upDir);
            CreateQuad(nearCorner, upDir, forwardDir);

            CreateQuad(farCorner, -rightDir, -forwardDir);
            CreateQuad(farCorner, -upDir, -rightDir);
            CreateQuad(farCorner, -forwardDir, -upDir);

            return this;
        }
    }
}