using UnityEngine;
using HyperGames;

public class CameraController : MonoBehaviour {

    // stuff like this should be in a input manager
    public LayerMask    interactiveMask;
    public Transform    rootAnchor;
    public Transform    rotateAnchor;
    public float        lowerX;
    public float        upperX;

    private bool        moveCamera;
    private Vector3     startPos;
    private Vector3     deltaPos;
    private float       pxPrMm; // pixels per mm
    private float       anglesPrMm; // angles per mm
    private Vector3     rootPos;
    private Quaternion  rotateRot;
    private Quaternion  rootRot;
    private Quaternion  rotationDelta;
    private Camera      cam;
    private float       zoomSpeed;

    void Awake() {
        cam = GetComponent<Camera>();
        moveCamera = false;
        pxPrMm = 64f;
        anglesPrMm = 10f;
        zoomSpeed = 15f;
        Messenger.AddListener<ScrollEvent>(OnScroll);
    }

    private void OnScroll(ScrollEvent scrollData) {
        cam.fieldOfView -= scrollData.direction.y * zoomSpeed * Time.deltaTime;
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 20f, 120f);
    }

    void Update () {
        if (Input.GetMouseButtonDown(0)) {
            if (!HitInteractiveObject()) {
                moveCamera = true;
                startPos = Input.mousePosition;
                rootPos = rootAnchor.position;
                rootRot = rootAnchor.rotation;
                rotateRot = rotateAnchor.rotation;
            }
        }

        if (Input.GetMouseButton(0) && moveCamera) {
            deltaPos = Input.mousePosition - startPos;
            deltaPos.x /= pxPrMm;
            deltaPos.y /= pxPrMm;
            RotateY();
            RotateX();
        }
    }

    private void RotateX() {
        rotateAnchor.localRotation = Quaternion.Euler(
            Cap(rotateRot.eulerAngles.x + NormalizeDelta(-deltaPos.y), lowerX, upperX),
            0f,
            0f);
    }

    private void RotateY() {
        rootAnchor.localRotation = Quaternion.Euler(
            0f,
            rootRot.eulerAngles.y + NormalizeDelta(deltaPos.x),
            0f);
    }

    private float Cap(float value, float min, float max) {
        return Mathf.Min(max, Mathf.Max(min, value));
    }

    private float NormalizeDelta(float axisValue) {
        return axisValue * anglesPrMm;
    }

    // this kind of test needs to be done in an input manager,
    // and when the manager decides what to do, use a delegate
    // or dispatch an event to the camera controller.
    private bool HitInteractiveObject() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, interactiveMask)) {
            return true;
        }
        return false;
    }
}
