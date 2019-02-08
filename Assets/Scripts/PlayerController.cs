using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
 * This class extends Monobehaviour which means it has access to monobehaviour methods and can be
 * added onto a gameobject in the inspector. All script components that are attached gameobjects
 * will extend Monobehaviour at some point.
 */
public class PlayerController : MonoBehaviour {


    /*
     *    These are public variables, meaning other scripts and classes will be able to access these fields.
     *    Making Monobehaviour variables public will also allow them to be viewed and initialized in the inspector.
     *    This allows us to easily change varables without having to change around the actual scripts whenever we want
     *    to tweak a float value.
     */
    
    //This is a reference to the main camera which we will rotate and use for movement later
    public Camera cam;

    //These are all floats that control camera movement
    public float xSensitivity, ySensitivity, maxXRot;

    //This is the rigidbody component for the player. It will be used to control the character using physics
    Rigidbody rb;

    bool grounded = false;

    //These are movement speeds used later to control the character
    public float jumpSpeed = 400, runSpeed; 


	// The start method runs when a script comes into existence and is enabled. Usually used for variable initiation or setup 

	void Start () {

        //Here we are initializing the varaible we defined before to the rigidbody component that is attached to the same
        //gameobject as this script. Now we have a reference to the rigidbody and can affect it with code.
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        grounded = Physics.Raycast(transform.position, Vector3.down, 1.25f);

        

        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) {

              float roty, rotx;
              rotx = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * xSensitivity;
              roty = cam.transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * ySensitivity;
              transform.localEulerAngles = new Vector3(transform.eulerAngles.x, rotx, transform.eulerAngles.z);
              cam.transform.localEulerAngles = new Vector3(roty,0,0);

        }


        Vector3 velocity = new Vector3();

        if (Input.GetAxis("Horizontal") != 0) {
            velocity += transform.right * Input.GetAxis("Horizontal") * runSpeed;
        }


        if (Input.GetAxis("Vertical") != 0) {
            velocity += transform.forward * Input.GetAxis("Vertical") * runSpeed;

        }

     
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (grounded) {
                rb.AddForce(Vector3.up * jumpSpeed);
                grounded = false;
            }
        } 

             rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        
        









    }
}
