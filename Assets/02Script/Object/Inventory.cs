using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItemData
{
    public int uid;
    public int itemID;
    public int type; // 0: onehand, 1: shield, 2:ranged, 3:helmet, 4:items, projectile
    public int amount;
    public float durability;
    public bool enchant;
}

[System.Serializable]
public class Inventory
{
    private int maxSlotCount = 32;
    public int MAXSlotCount { get => maxSlotCount; }

    private int curSlotCount = 0;
    public int CURSlotCount
    {
        get => curSlotCount;
        set => curSlotCount = value;
    }
    public bool IsFull
    {
        get { return curSlotCount >= maxSlotCount; }
    }
    public int DeltaSlotCount
    {
        get { return maxSlotCount - curSlotCount; }
    }

    [SerializeField]
    private List<InventoryItemData> items = new List<InventoryItemData>();

    public int FindIndexByUid(int uid)
    {
        curSlotCount = items.Count;
        for (int i = 0; i < curSlotCount; i++)
        {
            if (items[i].uid == uid)
            {
                return i;
            }
        }
        return -1;
    }

    public int FindIndexByItemID(int itemID)
    {
        curSlotCount = items.Count;
        for (int i = 0; i < curSlotCount; i++)
        {
            if (items[i].itemID == itemID)
            {
                return i;
            }
        }
        return -1;
    }



    public void AddItem(InventoryItemData newItem)
    {
        //int index = FindIndexByItemID(newItem.itemID);
        /*
        if (index <= -1)
        {
            if (GameManager.Inst.GetItemData(newItem.itemID, out TableEntity_Item item))
            {
                if (item.equip) // ÀåÂø¿©ºÎ
                {
                    newItem.uid = GameManager.Inst.PlayerUIDMaker;
                    newItem.amount = 1;
                    items.Add(newItem);
                    curSlotCount++;
                }
                else // first item
                {
                    newItem.uid = GameManager.Inst.PlayerUIDMaker;
                    items.Add(newItem);
                    curSlotCount++;
                }
            }
        }
        else if (index > -1)
        {
            items[index].amount += newItem.amount;
            if (items[index].amount > 99)
            {
                items[index].amount = 99;
            }
        }*/
    }

    public void DeleteItem(int itemID, int amount)
    {
        int index = FindIndexByItemID(itemID);
        if (-1 < index)
        {
            items[index].amount -= amount;
            if (items[index].amount < 1)
            {
                items.RemoveAt(index);
                curSlotCount--;
            }
        }
    }

    public void DeleteItem(InventoryItemData deleteItem)
    {
        int index = FindIndexByUid(deleteItem.uid);
        if (-1 < index)
        {
            items[index].amount -= deleteItem.amount;
            if (items[index].amount < 1)
            {
                items.RemoveAt(index);
                curSlotCount--;
            }
        }
    }
}
