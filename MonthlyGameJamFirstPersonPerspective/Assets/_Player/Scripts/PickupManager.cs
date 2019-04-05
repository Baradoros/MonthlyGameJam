using UnityEngine;

public class PickupManager : MonoBehaviour
{
    [SerializeField] private GameObject selectedObj;
    [SerializeField] private GameObject heldItem;

    private void OnEnable()
    {
        SelectionManager.OnObjectSelected += SelectionManagerOnOnObjectSelected;
    }
    
    private void OnDisable()
    {
        SelectionManager.OnObjectSelected -= SelectionManagerOnOnObjectSelected;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1")) 
        {
            if (heldItem == null) 
            {
                PickUpObject();
            }
            else 
            {
                DropHeldObject();
            }
        }
    }
    
    private void PickUpObject() 
    {
        if (selectedObj != null && heldItem == null) 
        {
            heldItem = selectedObj;
            Rigidbody heldRB = heldItem.GetComponent<Rigidbody>();
            if (heldRB != null) heldRB.isKinematic = true;
            heldItem.transform.SetParent(transform);
            heldItem.transform.localPosition = Vector3.zero;
        }
    }

    private void DropHeldObject()
    {
        if (heldItem != null)
        {
            heldItem.transform.SetParent(null);
            Rigidbody heldRB = heldItem.GetComponent<Rigidbody>();
            if (heldRB != null) heldRB.isKinematic = false;
            heldItem = null;
        }
    }

    // called when an object is selectable from the SelectionManager script
    private void SelectionManagerOnOnObjectSelected(GameObject obj)
    {
        // if (obj) Debug.Log(obj.name + " is selected");
        selectedObj = obj;
    }
}