using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    //Parameters
    public Item slotItem;
    private Sprite defaultSprite;


    private void Start()
    {
        defaultSprite = GetComponent<Image>().sprite;
    }


    private void Update()
    {
        ItemInSlot();
    }

    private void ItemInSlot()
    {
        if(transform.childCount >= 1)
        {
            slotItem = transform.GetChild(0).GetComponent<Item>();
            GetComponent<Image>().sprite = slotItem.itemSprite;
        }
        else
        {
            slotItem = null;
            GetComponent<Image>().sprite = defaultSprite;
        }
    }


   /* public void DropItem()
    {
        if(slotItem)
        {
            slotItem.transform.parent = null;
            slotItem.gameObject.SetActive(true);
            slotItem.transform.position = player.transform.position + new Vector3(0, 0, 2);
        }
    }*/
}
