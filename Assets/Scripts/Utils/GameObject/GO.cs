using UnityEngine;
using UnityEngine.Rendering;

namespace Mandarin {

    public class GO {

        public delegate void ComponentAdded<T>(T component) where T : Component;

        private GameObject current;

        static public GO Create() {
            return new GO();
        }

        static public GO Modify(GameObject go) {
            return new GO(go);
        }

        public GO() {
            current = new GameObject();
        }

        public GO(GameObject go) {
            current = go;
        }

        public GO SetName(string name) {
            current.name = name;
            return this;
        }

        public GO SetLayer(string name) {
            current.layer = LayerMask.NameToLayer(name);
            return this;
        }

        public GO SetParent(Transform parent) {
            current.transform.parent = parent;
            return this;
        }

        public GO SetActive(bool active) {
            current.SetActive(active);
            return this;
        }

        public GO SetMesh(Mesh mesh) {
            current.AddComponent<MeshFilter>().sharedMesh = mesh;
            return this;
        }

        public GO SetMaterial(Material mat) {
            GetSetComp<MeshRenderer>().sharedMaterial = mat;
            return this;
        }

        public GO ReceiveShadows(bool receiveShadows) {
            GetSetComp<MeshRenderer>().receiveShadows = receiveShadows;
            return this;
        }

        public GO CastShadows(ShadowCastingMode castShadows) {
            GetSetComp<MeshRenderer>().shadowCastingMode = castShadows;
            return this;
        }

        public GO SetScale(Vector3 scale) {
            current.transform.localScale = scale;
            return this;
        }

        public GO SetPosition(Vector3 position, bool local = false) {
            if (local) {
                current.transform.localPosition = position;
            } else {
                current.transform.position = position;
            }
            return this;
        }

        public GO SetRotation(Quaternion quaternion, bool local = false) {
            if (local) {
                current.transform.localRotation = quaternion;
            } else {
                current.transform.rotation = quaternion;
            }
            return this;
        }

        public GO AddBoxCollider(Vector3 size, Vector3 center = default(Vector3)) {
            BoxCollider collider = GetSetComp<BoxCollider>();
            collider.size = size;
            collider.center = center;
            return this;
        }

        public GO AddComponent<T>(ComponentAdded<T> callback = null) where T : Component {
            T component = current.AddComponent<T>();
            if (callback != null) {
                callback(component);
            }
            return this;
        }

        public GameObject Duplicate() {
            return GameObject.Instantiate(current) as GameObject;
        }

        private T GetSetComp<T>() {
            T comp = current.GetComponent<T>();
            if (comp == null) {
                comp = current.AddComponent<T>();
            }
            return comp;
        }

        public GameObject gameObject {
            get { return current; }
        }

        public Transform transform {
            get { return current.transform; }
        }
    }
}