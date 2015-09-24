using UnityEngine;
using System;
using System.Collections.Generic;

public class FPSController : MonoBehaviour {

    public float                    speed = 1f;
    public float                    sensitivityX = 15F;
    public float                    sensitivityY = 15F;
    public float                    minimumY = -60F;
    public float                    maximumY = 60F;

    private float                   rotationX = 0F;
    private float                   rotationY = 0F;
    private Quaternion              originalRotation;
    private Sequence<Vector3>       position;
    private Sequence<Quaternion>    rotation;

    void Awake() {
        originalRotation = transform.localRotation;

        InputConfig.Create(KeyCode.A, KeyCode.LeftArrow)
                   .SetCallback(() => { return -Vector3.right; });
        InputConfig.Create(KeyCode.D, KeyCode.RightArrow)
                   .SetCallback(() => { return  Vector3.right; });
        InputConfig.Create(KeyCode.W, KeyCode.UpArrow)
                   .SetCallback(() => { return  Vector3.forward; });
        InputConfig.Create(KeyCode.S, KeyCode.DownArrow)
                   .SetCallback(() => { return -Vector3.forward; });
        InputConfig.Create(KeyCode.LeftShift)
                   .SetCallback(() => { return  Vector3.up; });
        InputConfig.Create(KeyCode.Space)
                   .SetCallback(() => { return -Vector3.up; });

        position = Sequence<Vector3>.Create()
                    .Add(GetInput)
                    .Add(CalcNewPosition);

        rotation = Sequence<Quaternion>.Create()
                    .Add(CalcRotationX)
                    .Add(CalcRotationY);
    }

    void Update() {
        transform.localRotation = rotation.Run(originalRotation);
        Vector3 deltaPos = position.Run(Vector3.zero);
        transform.position = transform.position + deltaPos;
    }

    private Quaternion CalcRotationX(Quaternion rotation) {
        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        return rotation * Quaternion.AngleAxis(rotationX, Vector3.up);
    }

    private Quaternion CalcRotationY(Quaternion rotation) {
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = ClampAngle(rotationY, minimumY, maximumY);
        return rotation * Quaternion.AngleAxis(rotationY, -Vector3.right);
    }

    private Vector3 GetInput(Vector3 v3) {
        foreach (InputKey ik in InputConfig.Keys) {
            if (Key(ik.keyCodes)) {
                v3 = ik.callback();
            }
        }
        return v3;
    }

    private Vector3 CalcNewPosition(Vector3 moveDir) {
        Vector3 fwd = new Vector3(transform.forward.x, 0f, transform.forward.z);
        Vector3 right = Vector3.Cross(fwd, -Vector3.up);
        Vector3 velocity = moveDir.z * fwd +
                           moveDir.x * right +
                           moveDir.y * Vector3.up;
        return velocity.normalized * speed * Time.deltaTime;
    }

    public static float ClampAngle(float angle, float min, float max) {
        if (angle < -360F) {
            angle += 360F;
        }
        if (angle > 360F) {
            angle -= 360F;
        }
        return Mathf.Clamp(angle, min, max);
    }

    private bool Key(params KeyCode[] keyCodes) {
        foreach (KeyCode kc in keyCodes) {
            if (Input.GetKey(kc)) {
                return true;
            }
        }
        return false;
    }

    private class InputConfig {
        private static List<InputKey> inputKeys;

        static public InputKey Create(params KeyCode[] keys) {
            InputKey ik = new InputKey();
            ik.keyCodes = keys;
            if (inputKeys == null) {
                inputKeys = new List<InputKey>();
            }
            inputKeys.Add(ik);
            return ik;
        }

        static public List<InputKey> Keys {
            get { return inputKeys; }
        }
    }

    private class InputKey {
        public KeyCode[]        keyCodes;
        public Func<Vector3>    callback;

        public void SetCallback(Func<Vector3> cb) {
            callback = cb;
        }
    }

    private class Sequence<T> {
        private List<Func<T, T>> sequence;

        public Sequence() {
            sequence = new List<Func<T, T>>();
        }

        static public Sequence<T> Create() {
            return new Sequence<T>();
        }

        public Sequence<T> Add(Func<T, T> callback) {
            sequence.Add(callback);
            return this;
        }

        public T Run(T value) {
            T newValue = value;
            foreach (Func<T, T> callback in sequence) {
                newValue = callback(newValue);
            }
            return newValue;
        }
    }
}
