using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour
{
    GraphicRaycaster raycaster;
    PointerEventData pointer;
    EventSystem eventSystem;

    public UIItemSlot cursor;

    private void Awake()
    {
        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Set up new pointer event at mouse position
            pointer = new PointerEventData(eventSystem);
            pointer.position = Input.mousePosition;

            // Create list for raycast results
            List<RaycastResult> results = new List<RaycastResult>();

            // Raycast from pointer and pass results to list
            raycaster.Raycast(pointer, results);

            if (results.Count > 0 && results[0].gameObject.tag == "UIItemSlot")
            {
                ProcessClick(results[0].gameObject.GetComponent<UIItemSlot>());
            }
        }
    }

    private void ProcessClick(UIItemSlot clicked)
    {
        // Catch null errors
        if (clicked == null)
        {
            Debug.LogWarning("UI element tagged as UIItemSlot but has no UIItemSlot component");
            return;
        }

        // If item slots are different, swap them over
        if (!ItemSlot.Compare(cursor.itemSlot, clicked.itemSlot))
        {
            ItemSlot.Swap(cursor.itemSlot, clicked.itemSlot);
            cursor.RefreshSlot();
            return;
        }
        else // Items are the same
        {
            // If slots are empty, return
            if (!cursor.itemSlot.hasItem)
            {
                return;
            }
            // If items not stackable, return
            if (!cursor.itemSlot.item.isStackable)
            {
                return;
            }
            // If clicked slot is full, return
            if (clicked.itemSlot.amount == clicked.itemSlot.item.maxStack)
            {
                return;
            }

            // Add amounts of items and stack as many as possible into clicked slot, leaving rest with cursor
            int total = cursor.itemSlot.amount + clicked.itemSlot.amount;
            int maxStack = cursor.itemSlot.item.maxStack; // Cache maxStack amount

            if (total <= maxStack)
            {
                clicked.itemSlot.amount = total;
                cursor.itemSlot.Clear();
            }
            else
            {
                clicked.itemSlot.amount = maxStack;
                cursor.itemSlot.amount = total - maxStack;
            }

            cursor.RefreshSlot();
        }
    }
}
