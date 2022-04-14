using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventory;
    private Cam_follower cam;
    private float oldSenstivity;
    void Start()
    {
        cam = GameObject.Find("Camera Orentiation").GetComponent<Cam_follower>();
        oldSenstivity = cam.sensitivity;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            bool toggle = inventory.activeSelf;
            inventory.gameObject.SetActive(!toggle);
        }
        if(inventory.gameObject.activeSelf==true)
        {
            cam.sensitivity = 0;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
        else
        {
            cam.sensitivity = oldSenstivity;
            Cursor.visible = false;
            Time.timeScale = 1;
        }
    }
}
