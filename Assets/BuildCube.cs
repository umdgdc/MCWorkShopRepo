using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCube : MonoBehaviour {


    public void Start() {
        BuildChunk();
    }

    Chunk testChunk;
    public bool isEmpty(int x, int y, int z) {
        
        try {

            return chunkdata[x, y, z] == (byte)BlockType.AIR;
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
                                    y, (z + World.Instance.chunkSize) % World.Instance.chunkSize] == (byte)BlockType.AIR;
            }
            catch {

            }
            return false;
        }
    }


    
    MeshBuilder meshBuilder = new MeshBuilder();


    Vector3 upDir;
    Vector3 rightDir;
    Vector3 forwardDir;

    byte[,,] chunkdata;

    Vector3 offset;


    // Use this for initialization
    public void BuildChunk () {
        offset = new Vector3();
        chunkdata = new byte[16, 150, 16];

        upDir = Vector3.up;
         rightDir = Vector3.right;
         forwardDir = Vector3.forward;

         nearCorner = Vector3.zero;
         farCorner = upDir + rightDir + forwardDir;

        offset = new Vector3();
        Build((byte) BlockType.WATER);

        BuildMesh();

	}

    void Build(byte b) {


            //BuildQuad(meshBuilder, nearCorner + offset, forwardDir, rightDir, (int)b);
            //BuildQuad(meshBuilder, nearCorner + offset, rightDir, upDir, (int)b);
            //BuildQuad(meshBuilder, nearCorner + offset, upDir, forwardDir, (int)b);
            BuildQuad(meshBuilder, farCorner + offset, -rightDir, -forwardDir, (int)b);
            BuildQuad(meshBuilder, farCorner + offset, -upDir, -rightDir, (int)b);
            BuildQuad(meshBuilder, farCorner + offset, -forwardDir, -upDir, (int)b);

    }


    public void BuildMesh() {
        Mesh mesh = meshBuilder.CreateMesh();

        GetComponent<MeshFilter>().mesh = mesh;

    }


    Vector3 nearCorner;
    Vector3 farCorner;


    void BuildQuad(MeshBuilder meshBuilder, Vector3 offset,
    Vector3 widthDir, Vector3 lengthDir, int index) {
        Vector3 normal = Vector3.Cross(lengthDir, widthDir).normalized;

        meshBuilder.Vertices.Add(offset);
        meshBuilder.UVs.Add(World.blockUVs[index+1,0]);
        meshBuilder.Normals.Add(normal);

        meshBuilder.Vertices.Add(offset + lengthDir);
        meshBuilder.UVs.Add(World.blockUVs[index + 1, 2]);
        meshBuilder.Normals.Add(normal);

        meshBuilder.Vertices.Add(offset + lengthDir + widthDir);
        meshBuilder.UVs.Add(World.blockUVs[index + 1, 3]);
        meshBuilder.Normals.Add(normal);

        meshBuilder.Vertices.Add(offset + widthDir);
        meshBuilder.UVs.Add(World.blockUVs[index + 1, 1]);
        meshBuilder.Normals.Add(normal);

        int baseIndex = meshBuilder.Vertices.Count - 4;

        meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
        meshBuilder.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
    }



    /*
    public void Build(Vector3 offset, BlockType b) {

       
        BuildQuad(meshBuilder, nearCorner + offset, forwardDir, rightDir, (int)b);
        BuildQuad(meshBuilder, nearCorner + offset, rightDir, upDir, (int)b);
        BuildQuad(meshBuilder, nearCorner + offset, upDir, forwardDir, (int)b);
        BuildQuad(meshBuilder, farCorner + offset, -rightDir, -forwardDir, (int)b);
        BuildQuad(meshBuilder, farCorner + offset, -upDir, -rightDir, (int)b);
        BuildQuad(meshBuilder, farCorner + offset, -forwardDir, -upDir, (int)b);

        
    }
    */

}
