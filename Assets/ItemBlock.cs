using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class ItemBlock : MonoBehaviour {


    public void PrintCollection(List<Vector2> col) {
        int i = 0;

        Vector2[] vec2 = { new Vector2(0.0f, 0.0f), new Vector2(1.0f, 0.0f), new Vector2(0.0f, 1.0f), new Vector2(1.0f, 1.0f) };

        foreach (Vector2 item in col) {


           for (int x = 0; x < 4; x++) {
                if (item == vec2[x]) {
                    print("UV " + i.ToString() + " : " + x.ToString()); // Replace this with your version of printing
                    x = 20;
                }
            }

            i++;
        }
    }


    public Vector3 rotVec;

	// Use this for initialization
	void Start () {
		
	}
	
    public void MakeCube(byte b) {
        Mesh m = GetComponent<MeshFilter>().mesh;

        List<Vector2> lvec = new List<Vector2>();

        print("Cube Original UVs");
        m.GetUVs(0,lvec);
        PrintCollection(lvec);

        b += 1;
        lvec = new List<Vector2>();
        //1      
            lvec.Add(World.blockUVs[b, 0]);
            lvec.Add(World.blockUVs[b, 1]);
            lvec.Add(World.blockUVs[b, 2]);
            lvec.Add(World.blockUVs[b, 3]);
        //2
            lvec.Add(World.blockUVs[b, 2]);
            lvec.Add(World.blockUVs[b, 3]);
            lvec.Add(World.blockUVs[b, 2]);
            lvec.Add(World.blockUVs[b, 3]);
        //3
            lvec.Add(World.blockUVs[b, 0]);
            lvec.Add(World.blockUVs[b, 1]);
            lvec.Add(World.blockUVs[b, 0]);
            lvec.Add(World.blockUVs[b, 1]);
        //4
            lvec.Add(World.blockUVs[b, 0]);
            lvec.Add(World.blockUVs[b, 2]);
            lvec.Add(World.blockUVs[b, 3]);
            lvec.Add(World.blockUVs[b, 1]);
        //5
            lvec.Add(World.blockUVs[b, 0]);
            lvec.Add(World.blockUVs[b, 2]);
            lvec.Add(World.blockUVs[b, 3]);
            lvec.Add(World.blockUVs[b, 1]);
        //6
            lvec.Add(World.blockUVs[b, 0]);
            lvec.Add(World.blockUVs[b, 2]);
            lvec.Add(World.blockUVs[b, 3]);
            lvec.Add(World.blockUVs[b, 1]);
        //
        
        m.SetUVs(0,lvec);

        GetComponent<Rigidbody>().AddForce(Vector3.up * 100);

          


    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {

            //GetComponent<Rigidbody>().useGravity = false;
            Destroy(gameObject);
        }
    }


    // Update is called once per frame
    void Update () {
        transform.Rotate(rotVec);	
	}


}
