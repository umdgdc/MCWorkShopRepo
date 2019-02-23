using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour {


    public int maxHeight = 150;
    public float offsetX, offsetZ, scale = 2f;
    
   

    public int GenerateHeight(float x, float z) {

        float modx = x / (World.Instance.chunkSize * World.Instance.initialWorldSize) * scale + offsetX;
        float modz = z / (World.Instance.chunkSize * World.Instance.initialWorldSize) * scale + offsetZ;


        
        return (int)(maxHeight * Mathf.PerlinNoise(modx, modz));

    }
}
