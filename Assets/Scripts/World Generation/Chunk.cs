using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {

    public BlockType[,,] chunkdata;
    public Block[,,] blockData;
    public GameObject cursorObj;

    public IEnumerator ChunkRoutine(int x, int y, int z) {

        BuildChunk();

        Chunk next;
        if (World.Instance.WorldList.TryGetValue(new Vector3(x+1, y, z), out next)) {
            next.StartCoroutine(ChunkRoutine(x + 1, y, z));
        }

        yield return null;
        
    }

    public void BuildMap() {

        chunkdata = new BlockType[World.Instance.chunkSize, World.Instance.chunkheight, World.Instance.chunkSize];

        blockData = new Block[World.Instance.chunkSize, World.Instance.chunkheight, World.Instance.chunkSize];

        int currheight;

        for (int x = 0; x < World.Instance.chunkSize; x++)
            for (int z = 0; z < World.Instance.chunkSize; z++) {
                bool prevair = true;
                bool watertop = World.Instance.n.GenerateHeight(x + transform.position.x, z + transform.position.z) <= World.Instance.WaterHeight;
                for (int y = World.Instance.chunkheight - 1; y > 0; y--) {


                    
                    if (World.Instance.n.fBM3D((int)(x + transform.position.x), (int)(y + transform.position.y), (int)(z + transform.position.z), 0.1f, 3) < 0.42f) {
                        if (prevair && watertop && y < World.Instance.WaterHeight)
                            chunkdata[x, y, z] = BlockType.WATER;
                            else if (y < World.Instance.WaterHeight && prevair) chunkdata[x, y, z] = BlockType.WATER;
                            else chunkdata[x, y, z] = BlockType.AIR;

                    }

                    /*
                    
                    if (y <= World.Instance.n.GenerateStoneHeight(x+ transform.position.x, z + transform.position.z)) {

                        chunkdata[x, y, z] = BlockType.STONE;
                    }

                    */
                    

                      
                     else if (y <= World.Instance.n.OldGenerateHeight(x + transform.position.x, z + transform.position.z)) {
                        if (prevair) {
                            prevair = false;
                            chunkdata[x, y, z] = BlockType.GRASS;
                        }
                        else {
                            chunkdata[x, y, z] = BlockType.DIRT;
                        }

                    }
                    else if (y < World.Instance.WaterHeight) {
                        chunkdata[x, y, z] = BlockType.WATER;
                    }
                    else {
                        chunkdata[x, y, z] = BlockType.AIR;
                    }
                    
                }
            }

    }

    Chunk testChunk;

    public bool isEmpty(int x, int y, int z) {

        try {
            return chunkdata[x, y, z] == BlockType.AIR;
        }
        catch (System.IndexOutOfRangeException) {


            Vector3 OutsideChunk = (transform.position / World.Instance.chunkSize);


            if (x < 0) OutsideChunk.x -= 1;
            else if (x >= World.Instance.chunkSize) OutsideChunk.x += 1;

            if (z < 0) OutsideChunk.z -= 1;
            else if (z >= World.Instance.chunkSize) OutsideChunk.z += 1;

            World.Instance.WorldList.TryGetValue(OutsideChunk, out testChunk);


            try {

                return testChunk.chunkdata[(x + World.Instance.chunkSize) % World.Instance.chunkSize,
                                     y, (z + World.Instance.chunkSize) % World.Instance.chunkSize] == BlockType.AIR;
                
            } catch {
                
            }
            
            return false;
        }
    }

    public Vector3 vec = new Vector3();

    public void BuildMesh() {
        CI = new List<CombineInstance>();
        for (int x = 0; x < World.Instance.chunkSize; x++) {
            vec.x = x;
            for (int z = 0; z < World.Instance.chunkSize; z++) {
                vec.z = z;
                for (int y = 0; y < World.Instance.chunkheight; y++) {
                    vec.y = y;

                    cursorObj.transform.localPosition = vec - transform.position;
                    blockData[x, y, z] = new Block(chunkdata[x, y, z], new Vector3(x, y, z), gameObject, this);
                    blockData[x, y, z].Draw(isEmpty(x, y + 1, z), isEmpty(x, y - 1, z), isEmpty(x, y, z + 1), isEmpty(x, y, z - 1), isEmpty(x - 1, y, z), isEmpty(x + 1, y, z));



                }
            }
        }
    }

    // Use this for initialization

    public void BuildChunk() {
        BuildMesh();
        CombineQuads();
    }

    public List<CombineInstance> CI = new List<CombineInstance>();


    void CombineQuads() {
        //1. Combine all children meshes
        //MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = CI.ToArray();
        
        /*while (i < meshFilters.Length) {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }*/

        //2. Create a new mesh on the parent object
        MeshFilter mf = gameObject.GetComponent<MeshFilter>();
        mf.mesh = new Mesh();

        //3. Add combined meshes on children as the parent's mesh
        mf.mesh.CombineMeshes(combine);

        //4. Create a renderer for the parent
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.material = World.Instance.mat;

        MeshCollider Mc = GetComponent<MeshCollider>();
        Mc.sharedMesh = mf.mesh;
        


    }

    // Update is called once per frame
    void Update() {

    }
}