using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {

    public GameObject Dirt, Water, Stone, Grass;
    public CubeType[,,] chunkdata;


    public void BuildChunk() {

        chunkdata = new CubeType[World.chunksize, World.chunkheight, World.chunksize];


        for (int x = 0; x < World.chunksize; x++)
            for (int z = 0; z < World.chunksize; z++) {
                bool prevair = true;
                for (int y = World.chunkheight - 1; y > 0; y--) {
             
               


                    if (y < Utils.GenerateHeight(x + transform.position.x, z + transform.position.z) - 60) {
                        if (prevair) {
                            GameObject.Instantiate(Grass, transform.position + new Vector3(x, y, z), Quaternion.identity, gameObject.transform);
                            prevair = false;
                        }
                        else {
                            GameObject.Instantiate(Dirt, transform.position + new Vector3(x, y, z), Quaternion.identity, gameObject.transform);
                        }
                            chunkdata[x, y, z] = CubeType.DIRT;
                    } else if (y < World.WaterHeight) {
                        GameObject.Instantiate(Water, transform.position + new Vector3(x, y, z), Quaternion.identity, gameObject.transform);
                        chunkdata[x, y, z] = CubeType.WATER;
                    }

                }
            }

    }

	// Use this for initialization
	void Start () {
        BuildChunk();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
