using UnityEngine;

public struct BlockInfo {
    public string       meshName;
    public Quaternion   rotation;
}

public class SurfaceManager {

    static private int[] neighbourIDs = new int[] { 1, 2, 4, 8, 16, 32 };

    // Requires neighbours to be listed in correct order.
    // Returns the sum of each neighbour's ID.
    static public int GetID(Voxel[] neighbours) {
        int id = 0;
        for (int i=0; i<neighbours.Length; i++) {
            if (neighbours[i] == null) {
                continue;
            }
            id += neighbourIDs[i];
        }
        return id;
    }

    static public BlockInfo GetSurface(int id) {
        return GetBlockInfo(id);
    }

    static private BlockInfo GetBlockInfo(int id) {
        BlockInfo bi = new BlockInfo();
        bi.rotation = Quaternion.identity;

        switch (id) {
            case 0:
                bi.meshName = "0";
                break;

            case 1:
                bi.meshName = "1_2_4_8";
                break;
            case 2:
                bi.meshName = "1_2_4_8";
                bi.rotation = Quaternion.Euler(0f, 90f, 0f);
                break;
            case 4:
                bi.meshName = "1_2_4_8";
                bi.rotation = Quaternion.Euler(0f, 180f, 0f);
                break;
            case 8:
                bi.meshName = "1_2_4_8";
                bi.rotation = Quaternion.Euler(0f, 270f, 0f);
                break;

            case 16:
                bi.meshName = "16";
                break;

            case 32:
                bi.meshName = "32";
                break;

            case 3:
                bi.meshName = "3_6_12_9";
                break;
            case 6:
                bi.meshName = "3_6_12_9";
                bi.rotation = Quaternion.Euler(0f, 90f, 0f);
                break;
            case 12:
                bi.meshName = "3_6_12_9";
                bi.rotation = Quaternion.Euler(0f, 180f, 0f);
                break;
            case 9:
                bi.meshName = "3_6_12_9";
                bi.rotation = Quaternion.Euler(0f, 270f, 0f);
                break;

            case 17:
                bi.meshName = "17_18_20_24";
                break;
            case 18:
                bi.meshName = "17_18_20_24";
                bi.rotation = Quaternion.Euler(0f, 90f, 0f);
                break;
            case 20:
                bi.meshName = "17_18_20_24";
                bi.rotation = Quaternion.Euler(0f, 180f, 0f);
                break;
            case 24:
                bi.meshName = "17_18_20_24";
                bi.rotation = Quaternion.Euler(0f, 270f, 0f);
                break;

            case 33:
                bi.meshName = "33_34_36_40";
                break;
            case 34:
                bi.meshName = "33_34_36_40";
                bi.rotation = Quaternion.Euler(0f, 90f, 0f);
                break;
            case 36:
                bi.meshName = "33_34_36_40";
                bi.rotation = Quaternion.Euler(0f, 180f, 0f);
                break;
            case 40:
                bi.meshName = "33_34_36_40";
                bi.rotation = Quaternion.Euler(0f, 270f, 0f);
                break;

            case 5:
                bi.meshName = "5_10";
                break;
            case 10:
                bi.meshName = "5_10";
                bi.rotation = Quaternion.Euler(0f, 90f, 0f);
                break;

            case 48:
                bi.meshName = "48";
                break;

            case 11:
                bi.meshName = "11_7_14_13";
                break;
            case 7:
                bi.meshName = "11_7_14_13";
                bi.rotation = Quaternion.Euler(0f, 90f, 0f);
                break;
            case 14:
                bi.meshName = "11_7_14_13";
                bi.rotation = Quaternion.Euler(0f, 180f, 0f);
                break;
            case 13:
                bi.meshName = "11_7_14_13";
                bi.rotation = Quaternion.Euler(0f, 270f, 0f);
                break;

            case 21:
                bi.meshName = "21_26";
                break;
            case 26:
                bi.meshName = "21_26";
                bi.rotation = Quaternion.Euler(0f, 90f, 0f);
                break;

            case 37:
                bi.meshName = "37_42";
                break;
            case 42:
                bi.meshName = "37_42";
                bi.rotation = Quaternion.Euler(0f, 90f, 0f);
                break;

            case 49:
                bi.meshName = "49_50_52_56";
                break;
            case 50:
                bi.meshName = "49_50_52_56";
                bi.rotation = Quaternion.Euler(0f, 90f, 0f);
                break;
            case 52:
                bi.meshName = "49_50_52_56";
                bi.rotation = Quaternion.Euler(0f, 180f, 0f);
                break;
            case 56:
                bi.meshName = "49_50_52_56";
                bi.rotation = Quaternion.Euler(0f, 270f, 0f);
                break;

            case 35:
                bi.meshName = "35_38_44_41";
                break;
            case 38:
                bi.meshName = "35_38_44_41";
                bi.rotation = Quaternion.Euler(0f, 90f, 0f);
                break;
            case 44:
                bi.meshName = "35_38_44_41";
                bi.rotation = Quaternion.Euler(0f, 180f, 0f);
                break;
            case 41:
                bi.meshName = "35_38_44_41";
                bi.rotation = Quaternion.Euler(0f, 270f, 0f);
                break;

            case 19:
                bi.meshName = "19_22_28_25";
                break;
            case 22:
                bi.meshName = "19_22_28_25";
                bi.rotation = Quaternion.Euler(0f, 90f, 0f);
                break;
            case 28:
                bi.meshName = "19_22_28_25";
                bi.rotation = Quaternion.Euler(0f, 180f, 0f);
                break;
            case 25:
                bi.meshName = "19_22_28_25";
                bi.rotation = Quaternion.Euler(0f, 270f, 0f);
                break;

            case 43:
                bi.meshName = "43_39_46_45";
                break;
            case 39:
                bi.meshName = "43_39_46_45";
                bi.rotation = Quaternion.Euler(0f, 90f, 0f);
                break;
            case 46:
                bi.meshName = "43_39_46_45";
                bi.rotation = Quaternion.Euler(0f, 180f, 0f);
                break;
            case 45:
                bi.meshName = "43_39_46_45";
                bi.rotation = Quaternion.Euler(0f, 270f, 0f);
                break;

            case 27:
                bi.meshName = "27_23_30_29";
                break;
            case 23:
                bi.meshName = "27_23_30_29";
                bi.rotation = Quaternion.Euler(0f, 90f, 0f);
                break;
            case 30:
                bi.meshName = "27_23_30_29";
                bi.rotation = Quaternion.Euler(0f, 180f, 0f);
                break;
            case 29:
                bi.meshName = "27_23_30_29";
                bi.rotation = Quaternion.Euler(0f, 270f, 0f);
                break;

            case 51:
                bi.meshName = "51_54_60_57";
                break;
            case 54:
                bi.meshName = "51_54_60_57";
                bi.rotation = Quaternion.Euler(0f, 90f, 0f);
                break;
            case 60:
                bi.meshName = "51_54_60_57";
                bi.rotation = Quaternion.Euler(0f, 180f, 0f);
                break;
            case 57:
                bi.meshName = "51_54_60_57";
                bi.rotation = Quaternion.Euler(0f, 270f, 0f);
                break;

            case 53:
                bi.meshName = "53_58";
                break;
            case 58:
                bi.meshName = "53_58";
                bi.rotation = Quaternion.Euler(0f, 90f, 0f);
                break;

            case 15:
                bi.meshName = "15";
                break;

            case 59:
                bi.meshName = "59_55_62_61";
                break;
            case 55:
                bi.meshName = "59_55_62_61";
                bi.rotation = Quaternion.Euler(0f, 90f, 0f);
                break;
            case 62:
                bi.meshName = "59_55_62_61";
                bi.rotation = Quaternion.Euler(0f, 180f, 0f);
                break;
            case 61:
                bi.meshName = "59_55_62_61";
                bi.rotation = Quaternion.Euler(0f, 270f, 0f);
                break;

            case 47:
                bi.meshName = "47";
                break;

            case 31:
                bi.meshName = "31";
                break;
        }
        return bi;
    }
}
