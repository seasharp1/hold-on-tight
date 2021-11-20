using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]
public class Item : ScriptableObject
{
    // Item name
    public string itemName;

    // Description of item
    [TextArea]
    public string itemDescription;

    // Icon that shows up in inventory slots
    public Sprite icon;

    // Max amount of this item able to be stacked
    public int maxStack;

    // Max condition for item. If item doesn't degrade, set this to -1
    public int maxCondition;

    // Quick ways to check if item is stackable or degradable
    public bool isStackable { get { return (maxStack > 1); } }
    public bool isDegradable { get { return (maxCondition > -1); } }
}
