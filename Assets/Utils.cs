using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils {

    public static int maxHeight = 150;
    static float smooth = 0.01f;
    static int octaves = 9;
    static float persistence = 0.5f;
    public static float offsetX, offsetZ, scale = 2f;


    public static int GenerateHeight(float x, float z) {

        float modx = x / (World.chunksize * World.initialWorldSize) * scale + offsetX;
        float modz = z / (World.chunksize * World.initialWorldSize) * scale + offsetZ;



        return (int)(maxHeight * Mathf.PerlinNoise(modx, modz ));

    }

}