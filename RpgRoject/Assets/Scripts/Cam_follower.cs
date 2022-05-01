using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_follower : MonoBehaviour
{
    //Reference
    public GameObject player;


    //Parameters
    public float sensitivity;
    private float mouseX;
    private bool isInventroyOpen=false;

    void Start()
    {
        Cursor.visible = false;
        //Cursor.lockState=CursorLockMode.Locked;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            isInventroyOpen = !isInventroyOpen;
        }
        if(isInventroyOpen)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    void LateUpdate()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        transform.Rotate(Vector3.up *sensitivity * mouseX * Time.fixedDeltaTime);
        transform.position = player.transform.position;
    }
}
