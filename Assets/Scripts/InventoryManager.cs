using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public enum ItemType { BLOCK = 0, WEAPON = 1000, DECORATION = 2000, CRAFTING = 3000 }




[System.Serializable]
public class ItemStack {
    
    public int id;
    public ItemType iType;
    public int count;
    public string name;
    public Sprite icon;
    
    public ItemStack(ItemStack i) {
        id = i.id;
        iType = i.iType;
        icon = i.icon;
        name = i.name;
    }
    
}

public class InventoryManager : Singleton<InventoryManager> {

    public Camera cam;
    public GameObject itemBlock, TestClick, DragCursor;
    public TextMesh tm;
    public ISlot currStack;


    public ItemStack[] blockDB;
    public GameObject invWindow;
    public CraftingMaster cm;

    public int buttonIndex = 0;
    public int slotMax = 64;
    public ISlot[] invBar;

    public void SelectSlot(int btnindex) {
        invBar[btnindex].button.Select();
    }

    public void ChangeSlot(float dir) {
        if (dir > 0) buttonIndex++;
        else buttonIndex--;
        if (buttonIndex < 0) buttonIndex = invBar.Length - 1;
        else if (buttonIndex >= invBar.Length) buttonIndex = 0;
        SelectSlot(buttonIndex);
    }

    //Use this for initialization
    void Start() {

        currStack = DragCursor.GetComponent<ISlot>();
        invWindow.SetActive(false);
        //Read the inventory from a file
        foreach (ISlot i in invBar) {
            i.SetSlot(null, 0);
        }


        SelectSlot(0);


    }


    public bool AddToInventory(ItemType item, int id) {

        //Add to the slot
        foreach (ISlot i in invBar) {
            //New Item Case
            if (i.stack == null) {
                i.SetSlot(new ItemStack(blockDB[id]), 1);
                return true;
                //Add to Existing stack;
            }
            else if (i.stack.iType == item && i.stack.id == id && i.stack.count < slotMax) {
                i.AddItem(1);
                return true;
            }

        }
        return false;

    }

    // Update is called once per frame
    void Update() {

        if (World.Instance.gameState == GameState.WORLD) { 
           
            if (Input.GetButtonDown("Inventory")) {

                World.Instance.gameState = GameState.INVENTORY;
                invWindow.SetActive(true);
                DragCursor.SetActive(true);
                DragCursor.GetComponent<Image>().enabled = false;
                currStack.SetSlot(null, 0);


            } else if (Input.GetMouseButtonDown(0)) {

                DestroyBlock();

            }
            else if (Input.GetMouseButtonDown(1)) {
                if (invBar[buttonIndex].stack != null) {
                    if (AddBlock((byte)invBar[buttonIndex].stack.id)) {
                        invBar[buttonIndex].AddItem(-1);
                    }
                }

            }

        }
            
        else if (World.Instance.gameState == GameState.INVENTORY) {
            if (Input.GetButtonDown("Inventory")) {

                World.Instance.gameState = GameState.WORLD;
                invWindow.SetActive(false);
                DragCursor.GetComponent<Image>().enabled = false;
                DragCursor.SetActive(false);

            }

            DragCursor.transform.position = Input.mousePosition;

        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0) {

                ChangeSlot(Input.GetAxisRaw("Mouse ScrollWheel"));

        }
            

    }

    public void DestroyBlock() {

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 10)) {

            if (hit.collider.tag == "Chunk") {

                TestClick.transform.position = hit.point;

                Vector3 localIndex = hit.point - hit.transform.position;
                localIndex.y -= 0.5f * hit.normal.y;
                localIndex.x -= 0.5f * hit.normal.x;
                localIndex.z -= 0.5f * hit.normal.z;

                byte b = hit.transform.GetComponent<Chunk>().chunkdata[(int)(localIndex.x), (int)(localIndex.y), (int)(localIndex.z)];
                if (b == 0) b++;
                tm.text = localIndex.ToString() + " Normal: " + hit.normal.ToString() + " Byte " + b.ToString();

                GameObject gem = GameObject.Instantiate(itemBlock, hit.point, Quaternion.identity);
                gem.GetComponent<ItemBlock>().MakeCube(b);


                hit.transform.GetComponent<Chunk>().chunkdata[(int)(localIndex.x), (int)(localIndex.y), (int)(localIndex.z)] = (byte)BlockType.AIR;
                hit.transform.GetComponent<Chunk>().BuildChunk();

            }



        }

    }

    public bool AddBlock(byte id) {

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 10)) {

            if (hit.collider.tag == "Chunk") {

                TestClick.transform.position = hit.point;

                Vector3 localIndex = hit.point - hit.transform.position;
                localIndex.y += 0.5f * hit.normal.y;
                localIndex.x += 0.5f * hit.normal.x;
                localIndex.z += 0.5f * hit.normal.z;

                hit.transform.GetComponent<Chunk>().chunkdata[(int)localIndex.x, (int)localIndex.y, (int)localIndex.z] = id;
                hit.transform.GetComponent<Chunk>().BuildChunk();


                return true;
            }
        }

        return false;

    }

}

        
