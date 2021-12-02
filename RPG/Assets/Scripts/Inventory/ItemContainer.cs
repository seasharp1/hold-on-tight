using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemContainer : MonoBehaviour
{
    [Header("Container UI Elements")]
    public GameObject parentWindow;
    public Transform contentWindow; // GridLayoutGroup object
    public TextMeshProUGUI title;

    [Header("Container Details")]
    public string containerName;
    GameObject slotPrefab; // Prefab of the UIItemSlot object

    List<ItemSlot> items = new List<ItemSlot>();

    private void Start()
    {
        slotPrefab = Resources.Load<GameObject>("Prefabs/UIItemSlot");

        #region TempInventory
        Item[] tempItems = new Item[3];
        tempItems[0] = Resources.Load<Item>("Items/coin");
        tempItems[1] = Resources.Load<Item>("Items/healthpotion");
        tempItems[2] = Resources.Load<Item>("Items/sword");

        for (int i = 0; i < 14; ++i)
        {
            int index = Random.Range(0, 3);
            int amount = Random.Range(1, tempItems[index].maxStack);
            int condition = tempItems[index].maxCondition;

            items.Add(new ItemSlot(tempItems[index].name, amount, condition));
        }
        #endregion
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CloseContainer();
        } else if (Input.GetKeyDown(KeyCode.I))
        {
            OpenContainer(items);
        }
    }*/

    List<UIItemSlot> UISlots = new List<UIItemSlot>();

    public bool isParentWindowOpen { get { return (parentWindow.activeSelf); } }

    public void OpenContainer(List<ItemSlot> slots)
    {
        if (!isParentWindowOpen)
        {
            parentWindow.SetActive(true);

            title.text = containerName.ToUpper();

            // Loop through each item and instantiate UI element for it
            for (int i = 0; i < slots.Count; ++i)
            {
                GameObject newSlot = Instantiate(slotPrefab, contentWindow);

                newSlot.name = i.ToString();

                UISlots.Add(newSlot.GetComponent<UIItemSlot>());

                slots[i].AttachUI(UISlots[i]);
            }
        }
    }

    public void CloseContainer()
    {
        // Loop through each slot and detach/delete them
        foreach (UIItemSlot slot in UISlots)
        {
            slot.itemSlot.DetachUI();
            Destroy(slot.gameObject);
        }

        // Clear list and deactivate window
        UISlots.Clear();
        parentWindow.SetActive(false);
    }
}
