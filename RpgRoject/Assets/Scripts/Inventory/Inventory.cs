using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Slot[] slots;

    //Equipment
    public GameObject equipmentInventory;
    private bool hasItemInInventory;
    private GameObject item;
    public GameObject hand;


    private void Start()
    {
    }


    private void Update()
    {
        CheckIfEquipmentSlotHasItem();
        if (hasItemInInventory)
        {
            if(!hand.transform.GetComponentInChildren<Item>())
            {
                EquipItem();
            }
            else
            {
                if(hand.transform.GetComponentInChildren<Item>().itemName==item.transform.GetComponent<Item>().itemName)
                {
                    EquipItem();
                }
                else
                {
                    DeEquipItem();
                }
            }
        }
        else
        {
            DeEquipItem();
        }
    }


    //Pickup Item
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Item>())
        {
            collision.gameObject.SetActive(false);
            AddItem(collision.gameObject.GetComponent<Item>());
        }
    }


    private void AddItem(Item itemToAdd)
    {
        foreach (Slot slot in slots)
        {
            if (!slot.slotItem)
            {
                if (slot.transform.childCount < 1)
                {
                    itemToAdd.transform.SetParent(slot.transform, false);//keeps the scale
                    return;
                }
            }
        }
    }


    //DropItem in slot implement here


    //Equip Item
    private void CheckIfEquipmentSlotHasItem()
    {
        if (!(equipmentInventory.transform.GetChild(0).gameObject.transform.childCount == 0))
        {
            hasItemInInventory = true;
            item = equipmentInventory.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        }
        else
        {
            hasItemInInventory = false;
        }
    }


    private void EquipItem()
    {
        Debug.Log("Equipped: "+item.name);
        if(hand.transform.childCount<=5)
        {
            Instantiate(item, hand.transform.position, hand.transform.rotation, hand.transform).gameObject.SetActive(true);
        }
    }


    private void DeEquipItem()
    {
        Debug.Log("UnEquipped: ");
        if(hand.transform.GetComponentInChildren<Item>())
        {
            Destroy(hand.transform.GetComponentInChildren<Item>().gameObject);
        }
    }
}
