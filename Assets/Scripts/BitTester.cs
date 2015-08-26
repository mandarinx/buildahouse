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
        Debug.Log("Binary data: "+DataParser.GetBinaryString(v.data));

        Theme themeB = Theme.DEFAULT;
        DataParser.GetTheme(v.data, ref themeB);
        Debug.Log("Theme: "+themeB);

        // Change theme from space to western
        Debug.Log("Set theme: Theme.DEFAULT");
        DataParser.SetTheme(ref v.data, Theme.DEFAULT);
        Debug.Log("Binary data: "+DataParser.GetBinaryString(v.data));

        Theme themeA = Theme.DEFAULT;
        DataParser.GetTheme(v.data, ref themeA);
        Debug.Log("Theme: "+themeA);


        int blockID = 0;
        DataParser.GetBlock(v.data, ref blockID);
        Debug.Log("BlockID: "+blockID);

        Debug.Log("Set block ID: 0");
        DataParser.SetBlock(ref v.data, 0);
        Debug.Log("Binary data: "+DataParser.GetBinaryString(v.data));

        int blockIDNew = 0;
        DataParser.GetBlock(v.data, ref blockIDNew);
        Debug.Log("BlockID NEW: "+blockIDNew);


        int variantB = 0;
        DataParser.GetVariant(v.data, ref variantB);
        Debug.Log("Variant before: "+variantB);

        Debug.Log("Set variant: 2");
        DataParser.SetVariant(ref v.data, 2);
        Debug.Log("Binary data: "+DataParser.GetBinaryString(v.data));

        int variantA = 0;
        DataParser.GetVariant(v.data, ref variantA);
        Debug.Log("Variant after: "+variantA);


        Quaternion quatB = Quaternion.identity;
        DataParser.GetRotation(v.data, ref quatB);
        Debug.Log("Rotation before: "+quatB.eulerAngles);

        Debug.Log("Set rotation: 270, 0, 0");
        DataParser.SetRotation(ref v.data, Quaternion.Euler(270f, 0f, 0f));
        Debug.Log("Binary data: "+DataParser.GetBinaryString(v.data));

        Quaternion quatA = Quaternion.identity;
        DataParser.GetRotation(v.data, ref quatA);
        Debug.Log("Rotation after: "+quatA.eulerAngles);
    }

}
