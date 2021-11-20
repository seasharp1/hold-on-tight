using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemSlot : MonoBehaviour
{
    public bool isCursor;

    public ItemSlot itemSlot;
    public RectTransform slotRect;

    public Image icon;
    public TextMeshProUGUI amount;
    public Image condition;

    private void Awake()
    {
        itemSlot = new ItemSlot();
    }

    private void Update()
    {
        if (!isCursor) return;

        transform.position = Input.mousePosition;
    }

    public void RefreshSlot()
    {
        UpdateIcon();
        UpdateAmount();

        // Dragged items don't show condition bar, so don't update if this is the cursor
        if (!isCursor)
        {
            UpdateCondionBar();
        }
    }

    public void ClearSlot()
    {
        itemSlot = new ItemSlot();
        RefreshSlot();
    }

    private void UpdateIcon()
    {
        // Cases where icon is not needed
        if (itemSlot == null || !itemSlot.hasItem)
        {
            icon.enabled = false;
        }
        else // Else display icon
        {
            icon.enabled = true;
            icon.sprite = itemSlot.item.icon;
        }
    }

    private void UpdateAmount()
    {
        // Cases where amount is not needed
        if (itemSlot == null || !itemSlot.hasItem || itemSlot.amount < 2)
        {
            amount.enabled = false;
        }
        else // Else display amount
        {
            amount.enabled = true;
            amount.text = itemSlot.amount.ToString();
        }
    }

    private void UpdateCondionBar()
    {
        // Cases where condition bar is not needed
        if (itemSlot == null || !itemSlot.hasItem || !itemSlot.item.isDegradable)
        {
            condition.enabled = false;
        }
        else // Else display condition
        {
            condition.enabled = true;

            // Get normalised percentage of condition (0 -1)
            float conditionPercent = (float)itemSlot.condition / (float)itemSlot.item.maxCondition;

            // Multiply max width by percentage to get width
            float barWidth = slotRect.rect.width * conditionPercent;

            // Set width
            condition.rectTransform.sizeDelta = new Vector2(barWidth, condition.rectTransform.sizeDelta.y);

            // Change color from green to red as item becomes more degraded
            condition.color = Color.Lerp(Color.red, Color.green, conditionPercent);
        }
    }
}
