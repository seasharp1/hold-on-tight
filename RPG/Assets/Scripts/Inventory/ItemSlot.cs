using UnityEngine;

public class ItemSlot
{
    //l Item being stored in this slot
    public Item item;

    // Amount of this item
    private int _amount;
    public int amount
    {
        get { return _amount; }
        set 
        {
            if (item == null) _amount = 0;
            else if (value > item.maxStack) _amount = item.maxStack;
            else if (value < 1) _amount = 0;
            else _amount = value;
            RefreshUISlot();
        }
    }

    // Condition of item(s). Items with different conditions are treated as different items
    private int _condition;
    public int condition
    {
        get { return _condition; }
        set
        {
            if (item == null) _condition = 0; // No item = no condition
            else if (value > item.maxCondition) _condition = item.maxCondition;
            else _condition = value;
            RefreshUISlot();
        }
    }

    private UIItemSlot uiItemSlot;

    public void AttachUI(UIItemSlot uiSlot)
    { 
        uiItemSlot = uiSlot;
        uiItemSlot.itemSlot = this;
        RefreshUISlot();
    }

    public void DetachUI()
    {
        uiItemSlot.ClearSlot();
        uiItemSlot = null;
    }

    // Bool to check if ItemSlot is currently attached to a UIItemSlot
    public bool isAttachedToUI { get { return (uiItemSlot != null); } }

    public void RefreshUISlot()
    {
        // If item is not attached to UIItemSlot, there is nothing to refresh
        if (!isAttachedToUI)
        {
            return;
        }
        uiItemSlot.RefreshSlot();
    }

    public static bool Compare(ItemSlot slotA, ItemSlot slotB)
    {
        // If items or conditions are different then they are treated as different
        if (slotA.item != slotB.item || slotA.condition != slotB.condition)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static void Swap(ItemSlot slotA, ItemSlot slotB)
    {
        // Cache slotA's values
        Item _item = slotA.item;
        int _amount = slotA.amount;
        int _condition = slotA.condition;

        // Copy slotB's values into slotA
        slotA.item = slotB.item;
        slotA.amount = slotB.amount;
        slotA.condition = slotB.condition;

        // Copy cached values into slotB
        slotB.item = _item;
        slotB.amount = _amount;
        slotB.condition = _condition;

        // Refresh UISlots
        slotA.RefreshUISlot();
        slotB.RefreshUISlot();
    }

    public void Clear()
    {
        item = null;
        amount = 0;
        condition = 0;
        RefreshUISlot();
    }

    // Bool to check if slot is occupied
    public bool hasItem { get { return (item != null); } }

    private Item FindByName (string itemName)
    {
        itemName = itemName.ToLower(); // Make sure string is lower case (and file names as well)
        Item _item = Resources.Load<Item>(string.Format("Items/{0}", itemName)); // Load item from resources folder

        if (_item == null)
        {
            Debug.LogWarning(string.Format("Couldn't find \"{0}\". Item slot is empty.", itemName));
        }
        return _item;
    }

    // Constructor. Putting default values in params makes values optional
    public ItemSlot(string itemName, int _amount = 1, int _condition = 0)
    {
        Item _item = FindByName(itemName); // Get the item

        if (_item == null)
        {
            item = null;
            amount = 0;
            condition = 0;
            return;
        }
        else
        {
            item = _item;
            amount = _amount;
            condition = _condition;
        }
    }

    public ItemSlot()
    {
        item = null;
        amount = 0;
        condition = 0;
    }
}
