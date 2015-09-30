using UnityEngine;
using System;

static public class VoxelExtensions {

    // Free:      10 bits
    // Theme:      5 bits
    // Block:      5 bits
    // Variant:    6 bits
    // Rotation x: 2 bits
    // Rotation y: 2 bits
    // Rotation z: 2 bits

    static public Theme GetTheme(this Voxel v) {
        Theme theme = Theme.DEFAULT;
        if (IsSet(17, v.data)) {
            theme += 16;
        }
        if (IsSet(18, v.data)) {
            theme += 8;
        }
        if (IsSet(19, v.data)) {
            theme += 4;
        }
        if (IsSet(20, v.data)) {
            theme += 2;
        }
        if (IsSet(21, v.data)) {
            theme += 1;
        }
        return theme;
    }

    static public Voxel SetTheme(this Voxel v, Theme theme) {
        SetBits(ToBin((int)theme, 5), 17, ref v.data);
        return v;
    }

    static public BlockType GetBlockType(this Voxel v) {
        BlockType bt = BlockType.UNDEFINED;
        if (IsSet(12, v.data)) {
            bt += 16;
        }
        if (IsSet(13, v.data)) {
            bt += 8;
        }
        if (IsSet(14, v.data)) {
            bt += 4;
        }
        if (IsSet(15, v.data)) {
            bt += 2;
        }
        if (IsSet(16, v.data)) {
            bt += 1;
        }
        return bt;
    }

    static public Voxel SetBlockType(this Voxel v, int id) {
        SetBits(ToBin(id, 5), 12, ref v.data);
        return v;
    }

    static public Voxel SetBlockType(this Voxel v, BlockType bt) {
        SetBits(ToBin((int)bt, 5), 12, ref v.data);
        return v;
    }

    static public int GetVariant(this Voxel v) {
        int id = 0;
        if (IsSet(6, v.data)) {
            id += 32;
        }
        if (IsSet(7, v.data)) {
            id += 16;
        }
        if (IsSet(8, v.data)) {
            id += 8;
        }
        if (IsSet(9, v.data)) {
            id += 4;
        }
        if (IsSet(10, v.data)) {
            id += 2;
        }
        if (IsSet(11, v.data)) {
            id += 1;
        }
        return id;
    }

    static public Voxel SetVariant(this Voxel v, int id) {
        SetBits(ToBin(id, 6), 6, ref v.data);
        return v;
    }

    static public Quaternion GetRotation(this Voxel v) {
        return Quaternion.Euler(BitsToRotation(v.data, 5, 4),
                                BitsToRotation(v.data, 3, 2),
                                BitsToRotation(v.data, 1, 0));
    }

    static public Voxel SetRotation(this Voxel v, Quaternion quaternion) {
        char[] bits = new char[6];
        SetRotationIndex(quaternion.eulerAngles.x, ref bits, 0);
        SetRotationIndex(quaternion.eulerAngles.y, ref bits, 2);
        SetRotationIndex(quaternion.eulerAngles.z, ref bits, 4);
        // Reverse the bits before setting them, since bits are set
        // from right to left, and we want to read the rotation
        // x, y, z from left to right.
        Array.Reverse(bits);
        SetBits(new string(bits), 0, ref v.data);
        return v;
    }

    static private void SetRotationIndex(float angle, ref char[] bits, int pos) {
        if (angle >= 89f  && angle <= 91f) {
            bits[pos] = '1';
            bits[pos+1] = '0';
            return;
        }
        if (angle >= 179f && angle <= 181f) {
            bits[pos] = '0';
            bits[pos+1] = '1';
            return;
        }
        if (angle >= 269f && angle <= 271f) {
            bits[pos] = '1';
            bits[pos+1] = '1';
            return;
        }
        bits[pos] = '0';
        bits[pos+1] = '0';
    }

    static private string RotationIndex(float angle) {
        return angle >= 89f  && angle <= 91f  ? "01" :
               angle >= 179f && angle <= 181f ? "10" :
               angle >= 269f && angle <= 271f ? "11" : "00";
    }

    static private float BitsToRotation(int data, int first, int second) {
        float rot = 0f;
        if (IsSet(first, data)) {
            rot += 90f;
        }
        if (IsSet(second, data)) {
            rot += 180f;
        }
        return rot;
    }

    static private void SetBits(string bits, int fromBit, ref int data) {
        for (int i=0; i<bits.Length; i++) {
            if (bits[i] == '0') {
                Unset(i + fromBit, ref data);
            } else {
                Set(i + fromBit, ref data);
            }
        }
    }

    static private string ToBin(int value, int len) {
        return (len > 1 ? ToBin(value >> 1, len - 1) : null) + "01"[value & 1];
    }

    static private bool IsSet(int bitIndex, int data) {
        return (data & (1 << bitIndex)) != 0;
    }

    static private void Unset(int bitIndex, ref int data) {
        data &= ~(1 << bitIndex);
    }

    static private void Set(int bitIndex, ref int data) {
        data |= (1 << bitIndex);
    }

    static public string GetBinaryString(this Voxel v) {
        char[] b = new char[32];
        int pos = 31;
        int i = 0;

        while (i < 32) {
            if ((v.data & (1 << i)) != 0) {
                b[pos] = '1';
            } else {
                b[pos] = '0';
            }
            pos--;
            i++;
        }

        return new string(b);
    }

}
