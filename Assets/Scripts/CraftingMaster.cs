using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CraftingMaster : MonoBehaviour {


    public ISlot[] slots;
    
    public List<int> craftingdata;

    public ISlot resultBox;

    //public Dictionary<int[],int> recipes;
    
    public class Recipe {
        public List<int> formula;
        public int result;

        public Recipe(int[] form, int res) {
            result = res;
            formula = new List<int>(form);
        }
    }

    public Recipe[] recipes;

    public void BuildRecipes() {
        recipes = new Recipe[1];

        recipes[0] = new Recipe(new int[] { -1, -1, -1,
                                            -1, -1, -1,
                                             1,  3, -1 }, 2);

    }

    public void Start() {
        //slots = GetComponentsInChildren<ISlot>();
        BuildRecipes();
    }

    public void RecalculateRecipes() {
        craftingdata = new List<int>();
        for (int i = 0; i < slots.Length; i++) {
            if (slots[i].stack != null) {
                craftingdata.Add(slots[i].stack.id);
            }
            else craftingdata.Add(-1);
        }
        
        for (int i = 0; i < recipes.Length; i++) {

            if (craftingdata.SequenceEqual(recipes[i].formula)) {

                resultBox.SetSlot(new ItemStack(InventoryManager.Instance.blockDB[recipes[i].result]), 1);

                foreach (ISlot t in slots) {
                    t.AddItem(-1);
                }

            }

        }


    }
    
}
