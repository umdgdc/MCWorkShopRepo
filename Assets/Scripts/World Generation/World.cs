using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class World : Singleton<World> {

    
    public int seed;
    public Material mat;
    public int chunkSize = 16;
    public int worldheight = 1;
    public int chunkheight = 150;
    public int initialWorldSize = 2;
    public int WaterHeight = 15;
    public GameObject chunk;
    public Dictionary<Vector3, Chunk> WorldList;
    public Vector3 PlayerStartPostion;
    public Noise n;


    IEnumerator BuildWorld() {

        Random.seed = seed;

        WorldList = new Dictionary<Vector3, Chunk>();
        n = GetComponent<Noise>();
        n.offsetX = Random.Range(0f, 99999f);
        n.offsetZ = Random.Range(0f, 99999f);

        print("x = " + n.offsetX + " z = " + n.offsetZ);

        for (int x = 0; x < initialWorldSize; x++)
            for (int z = 0; z < initialWorldSize; z++)
                for (int y = 0; y < worldheight; y++) {

                    
                    
                    GameObject gem = GameObject.Instantiate(chunk, new Vector3(x * chunkSize, y * chunkheight, z * chunkSize) , Quaternion.identity);
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
