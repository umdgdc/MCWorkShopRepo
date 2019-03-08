using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum SlotType { CRAFTING, INVENTORY, RESULT }

public class ISlot : MonoBehaviour, IPointerClickHandler {


    public Button button;
    public Image img;
    public Text text;
    public ItemStack stack;
    public SlotType slot = SlotType.INVENTORY;
    

    public void AddItem(int dir) {
        if (stack == null) return;
        stack.count += dir;

        if (stack.count <= 0) {
            SetSlot(null, 0);
        }
        else {
            text.text = stack.count.ToString();
        }
    }

    public void SetSlot(ItemStack itemStack, int num) {
        if (itemStack == null) {
            img.enabled = false;
            text.text = "";
            stack = null;
        }
        else {
            img.enabled = true;
            text.text = num.ToString();
            img.sprite = itemStack.icon;
            stack = itemStack;
            stack.count = num;
        }


    }

    public void DragDrop() {


        if (SlotType.RESULT != slot && stack == null && InventoryManager.Instance.currStack.stack != null) {
            print("Here");
            SetSlot(InventoryManager.Instance.currStack.stack, InventoryManager.Instance.currStack.stack.count);
            InventoryManager.Instance.currStack.SetSlot(null, 0);
            InventoryManager.Instance.currStack.enabled = false;
            if (slot == SlotType.CRAFTING) {
                InventoryManager.Instance.cm.RecalculateRecipes();
            }
        } else if (stack != null && InventoryManager.Instance.currStack.stack == null) {
            InventoryManager.Instance.currStack.SetSlot(stack, stack.count);
            SetSlot(null, 0);
            InventoryManager.Instance.currStack.enabled = true;
        }

    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left)
            DragDrop();
        else if (eventData.button == PointerEventData.InputButton.Middle) { }

        else if (eventData.button == PointerEventData.InputButton.Right) { }

    }



    public void Start() {
        SetSlot(null, 0);
    }

}