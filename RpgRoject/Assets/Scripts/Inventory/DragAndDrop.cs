using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //Parameters
    private Transform objectToDrag;
    private Transform objectToReplace;
    private Vector3 orignalPosition;
    private Transform startParent;
    private Transform endParent;
    private Image imageToDrag;


    //Finding GameObject Under Mouse of Type Slot
    GameObject GameObjectUnderMouse()
    {
        GraphicRaycaster raycaster = GetComponent<GraphicRaycaster>();
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> result = new List<RaycastResult>();
        raycaster.Raycast(eventData, result);
        foreach (RaycastResult hitObject in result)
        {
            if (hitObject.gameObject.GetComponent<Slot>())
            {
                Debug.Log("Clicked Object:" + hitObject.gameObject.name);
                return hitObject.gameObject;
            }
        }
        return null;
    }


    //Getting Transform of GameObject Under Mouse
    Transform DraggaleTransformUderMouse()
    {
        GameObject clickedObject = GameObjectUnderMouse();
        if (clickedObject != null)
        {
            return clickedObject.transform;
        }
        return null;
    }


    //Setting Clicked Object Parametters
    public void OnBeginDrag(PointerEventData eventData)
    {
        objectToDrag = DraggaleTransformUderMouse();
        if(objectToDrag!=null)
        {
            orignalPosition = objectToDrag.position;
            startParent = objectToDrag.parent;
            imageToDrag = objectToDrag.GetComponent<Image>();
            imageToDrag.raycastTarget = false;
        }
    }


    //Setting Object position to Mouse Position and Setting Parameters for Replacable GameObject of type Slot
    public void OnDrag(PointerEventData eventData)
    {
        objectToDrag.position = eventData.position;
            objectToReplace = DraggaleTransformUderMouse();
            endParent = objectToReplace.parent;
    }


    //Replacing Objects Positions if Replacable Object Present or Snapping Back the Object if Replacable Object Absent
    public void OnEndDrag(PointerEventData eventData)
    {

        if (objectToReplace != null)
        {
            objectToDrag.position = objectToReplace.position;
            objectToDrag.parent = endParent;
            objectToReplace.position = orignalPosition;
            objectToReplace.parent = startParent;
        }
        else
        {
            objectToDrag.position = orignalPosition;
            objectToDrag.parent = startParent;
        }
        imageToDrag.raycastTarget = true;
        objectToDrag = null;
    }

  

    /* private void Update()
     {
         if (Input.GetMouseButtonDown(0))
         {
             objectToDrag = DraggaleTransformUderMouse();
             if (objectToDrag != null)
             {
                 isdragging = true;
                 orignalPosition = objectToDrag.position;
                 startParent = objectToDrag.parent;
                 imageToDrag = objectToDrag.GetComponent<Image>();
                 imageToDrag.raycastTarget = false;
             }
         }
         if(isdragging)
         {
             objectToDrag.position = Input.mousePosition;
         }
         if(Input.GetMouseButtonUp(0))
         {
             isdragging = false;
             Transform objectToReplace = DraggaleTransformUderMouse();
             endParent = objectToReplace.parent;
             if (objectToReplace != null)
             {
                 objectToDrag.position = objectToReplace.position;
                 objectToDrag.parent = endParent;
                 objectToReplace.position = orignalPosition;
                 objectToReplace.parent = startParent;
             }
             else
             {
                 if(objectToDrag!=null)
                 {
                     //objectToDrag.GetComponent<Slot>().DropItem();
                     objectToDrag.position = orignalPosition;
                     //objectToDrag.parent = startParent;
                 }

             }
             imageToDrag.raycastTarget = true;
             objectToDrag = null;
         }
     }*/
}
