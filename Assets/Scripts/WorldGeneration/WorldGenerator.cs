using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {

    public GameObject grass;

	// Use this for initialization
	void Start () {
        for (int x = 0; x < 5; x++)
            for (int y = 0; y < 5; y++) {
                GameObject.Instantiate(grass, new Vector3(x,0,y), Quaternion.identity);
            }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
