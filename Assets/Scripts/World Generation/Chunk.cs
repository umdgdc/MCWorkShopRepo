using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
    
    public BlockType[,,] chunkdata;
    public Block[,,] blockData;
    
    public void BuildMap() {

        chunkdata = new BlockType[World.Instance.chunkSize, World.Instance.chunkheight, World.Instance.chunkSize];

        blockData = new Block[World.Instance.chunkSize, World.Instance.chunkheight, World.Instance.chunkSize];

        for (int x = 0; x < World.Instance.chunkSize; x++)
            for (int z = 0; z < World.Instance.chunkSize; z++) {
                bool prevair = true;
                for (int y = World.Instance.chunkheight - 1; y > 0; y--) {
             
                    if (y < World.Instance.n.GenerateHeight(x + transform.position.x, z + transform.position.z)) {
                        if (prevair) {
                            prevair = false;
                            chunkdata[x, y, z] = BlockType.GRASS;
                        }
                        else {
                            chunkdata[x, y, z] = BlockType.DIRT;
                        }
                            
                    } else if (y < World.Instance.WaterHeight) {
                        chunkdata[x, y, z] = BlockType.STONE;
                    } else {
                        chunkdata[x, y, z] = BlockType.AIR;
                    }

                }
            }

    }

    public bool isEmpty(int x, int y, int z) {

        try {
            return chunkdata[x, y, z] == BlockType.AIR;
        }
        catch (System.IndexOutOfRangeException) { }
        
        return true;
    }


    public void BuildMesh() {
        for (int x = 0; x < World.Instance.chunkSize; x++)
            for (int z = 0; z < World.Instance.chunkSize; z++)
                for (int y = 0; y  < World.Instance.chunkheight; y++) {


                    blockData[x,y,z] = new Block(chunkdata[x,y,z], new Vector3(x,y,z), gameObject, this);
                    blockData[x, y, z].Draw(isEmpty(x, y + 1, z), isEmpty(x, y - 1, z), isEmpty(x, y, z + 1), isEmpty(x, y, z - 1), isEmpty(x - 1, y, z), isEmpty(x + 1, y, z));


                    
                }
    }
    
	// Use this for initialization
	
    public void BuildChunk() {
        BuildMap();
        BuildMesh();
        CombineQuads();
    }


    void CombineQuads() {
        //1. Combine all children meshes
        MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length) {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        //2. Create a new mesh on the parent object
        MeshFilter mf = (MeshFilter)gameObject.AddComponent(typeof(MeshFilter));
        mf.mesh = new Mesh();

        //3. Add combined meshes on children as the parent's mesh
        mf.mesh.CombineMeshes(combine);

        //4. Create a renderer for the parent
        MeshRenderer renderer = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = World.Instance.mat;

        MeshCollider Mc = gameObject.AddComponent<MeshCollider>();
        Mc.sharedMesh = mf.mesh;

        //5. Delete all uncombined children
        foreach (Transform quad in transform) {
            GameObject.Destroy(quad.gameObject);
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
