using UnityEngine;
using UnityEngine.Rendering;

namespace Mandarin {

    public class GOBuilder {

        public delegate void ComponentAdded<T>(T component) where T : Component;

        private GameObject current;

        static public GOBuilder Create() {
            return new GOBuilder();
        }

        static public GOBuilder Modify(GameObject go) {
            return new GOBuilder(go);
        }

        public GOBuilder() {
            current = new GameObject();
        }

        public GOBuilder(GameObject go) {
            current = go;
        }

        public GOBuilder SetName(string name) {
            current.name = name;
            return this;
        }

        public GOBuilder SetLayer(string name) {
            current.layer = LayerMask.NameToLayer(name);
            return this;
        }

        public GOBuilder SetParent(Transform parent) {
            current.transform.parent = parent;
            return this;
        }

        public GOBuilder SetActive(bool active) {
            current.SetActive(active);
            return this;
        }

        public GOBuilder SetMesh(Mesh mesh) {
            current.AddComponent<MeshFilter>().sharedMesh = mesh;
            return this;
        }

        public GOBuilder SetMaterial(Material mat,
                                       bool receiveShadows = true,
                                       ShadowCastingMode castShadow = ShadowCastingMode.On) {
            MeshRenderer mr = current.AddComponent<MeshRenderer>();
            mr.sharedMaterial = mat;
            mr.receiveShadows = receiveShadows;
            mr.shadowCastingMode = castShadow;
            return this;
        }

        public GOBuilder SetScale(Vector3 scale) {
            current.transform.localScale = scale;
            return this;
        }

        public GOBuilder SetPosition(Vector3 position, bool local = false) {
            if (local) {
                current.transform.localPosition = position;
            } else {
                current.transform.position = position;
            }
            return this;
        }

        public GOBuilder SetRotation(Quaternion quaternion, bool local = false) {
            if (local) {
                current.transform.localRotation = quaternion;
            } else {
                current.transform.rotation = quaternion;
            }
            return this;
        }

        public GOBuilder AddComponent<T>(ComponentAdded<T> callback = null) where T : Component {
            T component = current.AddComponent<T>();
            if (callback != null) {
                callback(component);
            }
            return this;
        }

        public GameObject Duplicate() {
            return GameObject.Instantiate(current) as GameObject;
        }

        public GOBuilder AddBoxCollider(Vector3 size, Vector3 center = default(Vector3)) {
            BoxCollider collider = current.AddComponent<BoxCollider>();
            collider.size = size;
            collider.center = center;
            collider = null;
            return this;
        }

        public GameObject gameObject {
            get { return current; }
        }

        public Transform transform {
            get { return current.transform; }
        }
    }
}