using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CubeType { DIRT, STONE, WATER };


public class World : MonoBehaviour {


    
    public static int chunksize = 10;
    public static int worldheight = 1;
    public static int chunkheight = 20;
    public static int initialWorldSize = 2;
    public static int WaterHeight = 10;
    public GameObject chunk;
    public Dictionary<Vector3, Chunk> WorldList;
    public Vector3 PlayerStartPostion;


    IEnumerator BuildWorld() {

        WorldList = new Dictionary<Vector3, Chunk>();

        Utils.offsetX = Random.Range(0f, 99999f);
        Utils.offsetZ = Random.Range(0f, 99999f);

        print("x = " + Utils.offsetX + " z = " + Utils.offsetZ);

        for (int x = 0; x < initialWorldSize; x++)
            for (int z = 0; z < initialWorldSize; z++)
                for (int y = 0; y < worldheight; y++) {

                    
                    
                    GameObject gem = GameObject.Instantiate(chunk, new Vector3(x * chunksize, y * chunkheight, z * chunksize) , Quaternion.identity);
                    WorldList.Add(new Vector3(x, y, z), gem.GetComponent<Chunk>());
                    gem.GetComponent<Chunk>().BuildChunk();
                    yield return new WaitForSeconds(0.5f);


                }



    }

    // Use this for initialization
    void Start () {
        StartCoroutine(BuildWorld());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
