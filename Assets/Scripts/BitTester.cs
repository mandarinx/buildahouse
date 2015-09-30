using UnityEngine;
using System;

public class BitTester : MonoBehaviour {

    void Start () {
        Voxel v = new Voxel();

        // Theme:      5 bits
        // Block:      5 bits
        // Variant:    6 bits
        // Rotation x: 2 bits
        // Rotation y: 2 bits
        // Rotation z: 2 bits

        string theme = "11111";
        string block = "11111";
        string variant = "111111";
        string rotX = "11";
        string rotY = "01";
        string rotZ = "00";
        string data = theme + block + variant + rotX + rotY + rotZ;
        v.data = Convert.ToInt32(data, 2);
        Debug.Log("Raw data: "+v.data);
        Debug.Log("Binary data: "+ v.GetBinaryString());

        // Change theme from space to western
        Debug.Log("Theme: "+v.GetTheme());
        Debug.Log("Set theme: Theme.DEFAULT");
        v.SetTheme(Theme.DEFAULT);
        Debug.Log("Binary data: "+v.GetBinaryString());
        Debug.Log("Theme: "+v.GetTheme());


        Debug.Log("BlockID: "+v.GetBlockType());
        Debug.Log("Set block ID: 0");
        v.SetBlockType(0);
        Debug.Log("Binary data: "+v.GetBinaryString());
        Debug.Log("BlockID NEW: "+v.GetBlockType());


        Debug.Log("Variant before: "+v.GetVariant());
        Debug.Log("Set variant: 2");
        v.SetVariant(2);
        Debug.Log("Binary data: "+v.GetBinaryString());
        Debug.Log("Variant after: "+v.GetVariant());


        Debug.Log("Rotation before: "+v.GetRotation().eulerAngles);
        Debug.Log("Set rotation: 270, 0, 0");
        v.SetRotation(Quaternion.Euler(270f, 0f, 0f));
        Debug.Log("Binary data: "+v.GetBinaryString());
        Debug.Log("Rotation after: "+v.GetRotation().eulerAngles);
    }

}
