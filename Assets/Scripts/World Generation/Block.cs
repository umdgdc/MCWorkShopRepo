﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Block {

    enum Cubeside { BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK };

    byte bType;
    public bool isSolid;
    Chunk owner;
    GameObject parent;
    Vector3 position;

    public Block(byte b, Vector3 pos, GameObject p, Chunk o) {
        bType = b;
        owner = o;
        parent = p;
        position = pos;
        if (bType == (byte)BlockType.AIR)
            isSolid = false;
        else
            isSolid = true;
    }

    void CreateQuad(Cubeside side) {
        Mesh mesh = new Mesh();
        mesh.name = "ScriptedMesh" + side.ToString();

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uvs = new Vector2[4];
        int[] triangles = new int[6];

        //all possible UVs
        Vector2 uv00;
        Vector2 uv10;
        Vector2 uv01;
        Vector2 uv11;

        if (bType == (byte)BlockType.GRASS && side == Cubeside.TOP) {
            uv00 = World.blockUVs[0, 0];
            uv10 = World.blockUVs[0, 1];
            uv01 = World.blockUVs[0, 2];
            uv11 = World.blockUVs[0, 3];
        }
        else if (bType == (byte)BlockType.GRASS && side == Cubeside.BOTTOM) {
            uv00 = World.blockUVs[(int)(BlockType.DIRT + 1), 0];
            uv10 = World.blockUVs[(int)(BlockType.DIRT + 1), 1];
            uv01 = World.blockUVs[(int)(BlockType.DIRT + 1), 2];
            uv11 = World.blockUVs[(int)(BlockType.DIRT + 1), 3];
        }
        else {
            uv00 = World.blockUVs[(int)(bType + 1), 0];
            uv10 = World.blockUVs[(int)(bType + 1), 1];
            uv01 = World.blockUVs[(int)(bType + 1), 2];
            uv11 = World.blockUVs[(int)(bType + 1), 3];
        }

        //all possible vertices 
        Vector3 p0 = new Vector3(-0.5f, -0.5f, 0.5f);
        Vector3 p1 = new Vector3(0.5f, -0.5f, 0.5f);
        Vector3 p2 = new Vector3(0.5f, -0.5f, -0.5f);
        Vector3 p3 = new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 p4 = new Vector3(-0.5f, 0.5f, 0.5f);
        Vector3 p5 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 p6 = new Vector3(0.5f, 0.5f, -0.5f);
        Vector3 p7 = new Vector3(-0.5f, 0.5f, -0.5f);

        switch (side) {
            case Cubeside.BOTTOM:
                vertices = new Vector3[] { p0, p1, p2, p3 };
                normals = new Vector3[] {Vector3.down, Vector3.down,
                                            Vector3.down, Vector3.down};
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.TOP:
                vertices = new Vector3[] { p7, p6, p5, p4 };
                normals = new Vector3[] {Vector3.up, Vector3.up,
                                            Vector3.up, Vector3.up};
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.LEFT:
                vertices = new Vector3[] { p7, p4, p0, p3 };
                normals = new Vector3[] {Vector3.left, Vector3.left,
                                            Vector3.left, Vector3.left};
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.RIGHT:
                vertices = new Vector3[] { p5, p6, p2, p1 };
                normals = new Vector3[] {Vector3.right, Vector3.right,
                                            Vector3.right, Vector3.right};
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.FRONT:
                vertices = new Vector3[] { p4, p5, p1, p0 };
                normals = new Vector3[] {Vector3.forward, Vector3.forward,
                                            Vector3.forward, Vector3.forward};
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.BACK:
                vertices = new Vector3[] { p6, p7, p3, p2 };
                normals = new Vector3[] {Vector3.back, Vector3.back,
                                            Vector3.back, Vector3.back};
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();

        CombineInstance ci = new CombineInstance();
        ci.mesh = mesh;
        ci.transform = owner.cursorObj.transform.localToWorldMatrix;

        owner.CI.Add(ci);

        //GameObject quad = new GameObject("Quad");
        //quad.transform.position = position;
        //quad.transform.parent = this.parent.transform;

        //MeshFilter meshFilter = (MeshFilter)quad.AddComponent(typeof(MeshFilter));
        //meshFilter.mesh = mesh;

    }

    int ConvertBlockIndexToLocal(int i) {
        if (i == -1)
            i = World.Instance.chunkSize - 1;
        else if (i == World.Instance.chunkSize)
            i = 0;
        return i;
    }


    public void Draw(bool top, bool bot, bool front, bool back, bool left, bool right) {
        if (bType == (byte)BlockType.AIR) return;

        if (front)
            CreateQuad(Cubeside.FRONT);
        if (back)
            CreateQuad(Cubeside.BACK);
        if (top)
            CreateQuad(Cubeside.TOP);
        if (bot)
            CreateQuad(Cubeside.BOTTOM);
        if (left)
            CreateQuad(Cubeside.LEFT);
        if (right)
            CreateQuad(Cubeside.RIGHT);
    }
}