using UnityEngine;

[ExecuteInEditMode]
public class WaterFlow : MonoBehaviour {
    public float m_SpeedU = 0.1f;
    public float m_SpeedV = -0.1f;
    private Renderer r;

    void Update() {
        r = GetComponent<Renderer>();
        if (r == null) {
            return;
        }

        r.sharedMaterial.mainTextureOffset = new Vector2(
            m_SpeedU * Mathf.Sin(Time.time),
            m_SpeedV * Mathf.Sin(Time.time));
    }
}