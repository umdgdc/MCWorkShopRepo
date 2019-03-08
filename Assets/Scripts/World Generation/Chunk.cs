using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {

    public byte[,,] chunkdata;
    public Block[,,] blockData;
    public GameObject cursorObj;

    public IEnumerator ChunkRoutine(int x, int y, int z) {

        //BuildChunk();

        Chunk next;
        if (World.Instance.WorldList.TryGetValue(new Vector3(x+1, y, z), out next)) {
            next.StartCoroutine(ChunkRoutine(x + 1, y, z));
        }

        yield return null;
        
    }

    public void BuildMap() {

        chunkdata = new byte[World.Instance.chunkSize, World.Instance.chunkheight, World.Instance.chunkSize];

        blockData = new Block[World.Instance.chunkSize, World.Instance.chunkheight, World.Instance.chunkSize];

        int currheight;

        for (int x = 0; x < World.Instance.chunkSize; x++)
            for (int z = 0; z < World.Instance.chunkSize; z++) {
                bool prevair = true;
                bool watertop = World.Instance.n.GenerateHeight(x + transform.position.x, z + transform.position.z) <= World.Instance.WaterHeight;
                for (int y = World.Instance.chunkheight - 1; y > 0; y--) {


                    
                    /*if (World.Instance.n.fBM3D((int)(x + transform.position.x), (int)(y + transform.position.y), (int)(z + transform.position.z), 0.1f, 3) < 0.42f) {
                        if (prevair && watertop && y < World.Instance.WaterHeight)
                            chunkdata[x, y, z] = (byte)BlockType.WATER;
                            else if (y < World.Instance.WaterHeight && prevair) chunkdata[x, y, z] = (byte)BlockType.WATER;
                            else chunkdata[x, y, z] = (byte)BlockType.AIR;

                    }
                    
                     else*/ if (y <= World.Instance.n.OldGenerateHeight(x + transform.position.x, z + transform.position.z)) {
                        if (prevair) {
                            prevair = false;
                            chunkdata[x, y, z] = (byte)BlockType.GRASS;
                        }
                        else {
                            chunkdata[x, y, z] = (byte)BlockType.DIRT;
                        }

                    }
                    else if (y < World.Instance.WaterHeight) {
                        chunkdata[x, y, z] = (byte)BlockType.WATER;
                    }
                    else {
                        chunkdata[x, y, z] = (byte)BlockType.AIR;
                    }
                    
                }
            }

    }

    Chunk testChunk;


    /* This method is  used to check if a given block has a neighbour in the chunkdata 2D array
     * it is used to determine what faces of a cube should be drawn.
     */
    public bool isEmpty(int x, int y, int z) {



        /* Try catch is used to catch if something goes out of bounds this is more efficient than doing
         * 6 range comparisons for every face of every cube to check if x,y,z are in bound
         */
        try {

            return chunkdata[x, y, z] == (byte)BlockType.AIR;
        }
        catch (System.IndexOutOfRangeException) {

            //Now that we know we are checking outside of the chunk we have to check the adjacent junk instead

            Vector3 OutsideChunk = (transform.position / World.Instance.chunkSize);


            if (x < 0) OutsideChunk.x -= 1;
            else if (x >= World.Instance.chunkSize) OutsideChunk.x += 1;

            if (z < 0) OutsideChunk.z -= 1;
            else if (z >= World.Instance.chunkSize) OutsideChunk.z += 1;

            World.Instance.WorldList.TryGetValue(OutsideChunk, out testChunk);


            try {

                return testChunk.chunkdata[(x + World.Instance.chunkSize) % World.Instance.chunkSize,
                                     y, (z + World.Instance.chunkSize) % World.Instance.chunkSize] == (byte)BlockType.AIR;
                
            } catch {
                
            }
            
            return false;
        }
    }

    public Vector3 vec = new Vector3();


    /*
     * This function builds a list of meshes that will be combined into 
     * 
     */

/*    
    public void BuildChunk() {
        CI = new List<CombineInstance>();
        for (int x = 0; x < World.Instance.chunkSize; x++) {
            vec.x = x;
            for (int z = 0; z < World.Instance.chunkSize; z++) {
                vec.z = z;
                for (int y = 0; y < World.Instance.chunkheight; y++) {
                    vec.y = y;

                    cursorObj.transform.localPosition = vec - transform.position;
                    blockData[x, y, z] = new Block((byte)chunkdata[x, y, z], new Vector3(x, y, z), gameObject, this);
                    blockData[x, y, z].Draw(isEmpty(x, y + 1, z), isEmpty(x, y - 1, z), isEmpty(x, y, z + 1), isEmpty(x, y, z - 1), isEmpty(x - 1, y, z), isEmpty(x + 1, y, z));



                }
            }
        }

        CombineQuads();
    }

        */
    
    MeshBuilder meshBuilder = new MeshBuilder();


    Vector3 upDir;
    Vector3 rightDir;
    Vector3 forwardDir;
    Vector3 botCorner;
   
    Vector3 Ioffset;


    // Use this for initialization
    public void BuildChunk() {
        meshBuilder = new MeshBuilder();
        Ioffset = new Vector3();
       // chunkdata = new byte[World.Instance.chunkSize, World.Instance.chunkheight, World.Instance.chunkSize];

        upDir = Vector3.up;
        rightDir = Vector3.right;
        forwardDir = Vector3.forward;

        nearCorner = Vector3.zero;
        farCorner = upDir + rightDir + forwardDir;
        botCorner = rightDir + forwardDir;


        for (int x = 0; x < World.Instance.chunkSize; x++) {
            Ioffset.x = x;
            for (int y = 0; y < World.Instance.chunkheight; y++) {
                Ioffset.y = y;
                for (int z = 0; z < World.Instance.chunkSize; z++) {
                    Ioffset.z = z;
                    byte b = chunkdata[x, y, z];
                    if (b != (byte)BlockType.AIR) {

                        if (isEmpty(x, y-1, z)) {
                            if (b == 0)
                            BuildQuad(meshBuilder, nearCorner + Ioffset, forwardDir, rightDir, 1);
                            else
                            BuildQuad(meshBuilder, nearCorner + Ioffset, forwardDir, rightDir, (int)b);
                        }
                        //Back
                        if (isEmpty(x, y, z-1))
                            BuildQuad(meshBuilder, nearCorner + Ioffset, rightDir, upDir, (int)b);
                        if (isEmpty(x - 1, y, z))
                            BuildQuad(meshBuilder, forwardDir + Ioffset, -forwardDir, upDir, (int)b);
                        if (isEmpty(x, y + 1, z)) {

                            if (b == 0)
                            BuildQuad(meshBuilder, farCorner + Ioffset, -rightDir, -forwardDir, 4);
                            else
                            BuildQuad(meshBuilder, farCorner + Ioffset, -rightDir, -forwardDir, (int)b);

                        }
                        if (isEmpty(x, y, z + 1)) 
                            BuildQuad(meshBuilder, botCorner + Ioffset, -rightDir, upDir, (int)b);
                        if (isEmpty(x + 1, y, z))
                            BuildQuadI(meshBuilder, farCorner + Ioffset, -forwardDir, -upDir, (int)b);

                    }

                }
            }

        }

        BuildMesh();

    }

    public void BuildMesh() {
        Mesh mesh = meshBuilder.CreateMesh();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = World.Instance.mat;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }


    Vector3 nearCorner;
    Vector3 farCorner;


    void BuildQuad(MeshBuilder meshBuilder, Vector3 offset,
    Vector3 widthDir, Vector3 lengthDir, int index) {
        Vector3 normal = Vector3.Cross(lengthDir, widthDir).normalized;

        meshBuilder.Vertices.Add(offset);
        meshBuilder.UVs.Add(World.blockUVs[index , 0]);
        meshBuilder.Normals.Add(normal);

        meshBuilder.Vertices.Add(offset + lengthDir);
        meshBuilder.UVs.Add(World.blockUVs[index, 2]);
        meshBuilder.Normals.Add(normal);

        meshBuilder.Vertices.Add(offset + lengthDir + widthDir);
        meshBuilder.UVs.Add(World.blockUVs[index, 3]);
        meshBuilder.Normals.Add(normal);

        meshBuilder.Vertices.Add(offset + widthDir);
        meshBuilder.UVs.Add(World.blockUVs[index , 1]);
        meshBuilder.Normals.Add(normal);

        int baseIndex = meshBuilder.Vertices.Count - 4;

        meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
        meshBuilder.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
    }


    void BuildQuadI(MeshBuilder meshBuilder, Vector3 offset,
    Vector3 widthDir, Vector3 lengthDir, int index) {
        Vector3 normal = Vector3.Cross( widthDir, lengthDir).normalized;
        

        meshBuilder.Vertices.Add(offset);
        meshBuilder.UVs.Add(World.blockUVs[index, 3]);
        meshBuilder.Normals.Add(normal);

        meshBuilder.Vertices.Add(offset + lengthDir);
        meshBuilder.UVs.Add(World.blockUVs[index, 1]);
        meshBuilder.Normals.Add(normal);

        meshBuilder.Vertices.Add(offset + lengthDir + widthDir);
        meshBuilder.UVs.Add(World.blockUVs[index, 0]);
        meshBuilder.Normals.Add(normal);

        meshBuilder.Vertices.Add(offset + widthDir);
        meshBuilder.UVs.Add(World.blockUVs[index, 2]);
        meshBuilder.Normals.Add(normal);

        int baseIndex = meshBuilder.Vertices.Count - 4;

        meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
        meshBuilder.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
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