using UnityEditor;
using UnityEngine;
using Statnett.Utils;

public class AssetImporter : AssetPostprocessor {

    public void OnPreprocessModel () {
        if (!assetPath.Contains("Resources/Meshes/")) {
            return;
        }
        ModelImporter importer = assetImporter as ModelImporter;
        importer.importMaterials = false;
        importer.animationType = ModelImporterAnimationType.None;
    }

    public void OnPostprocessModel(GameObject go) {
        MeshFilter filter = go.GetComponent<MeshFilter>();
        if (filter == null) {
            return;
        }
        Mesh mesh = filter.sharedMesh;
        MeshUtilsEditor.DuplicateMesh(mesh, assetPath.Replace(".fbx", ""));
    }
}