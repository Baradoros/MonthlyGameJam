using UnityEngine;

public class PickupManager : MonoBehaviour
{
    [Header("Rotation Variables")]
    public PlayerController PlayerController;
    public float RotationSpeed;

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
                PlayerController.IsRotatingHeldItem = false;
            }
        }

        //Rotates character object
        if (Input.GetButtonDown("Fire2") && heldItem != null)
            PlayerController.IsRotatingHeldItem = true;
        if (Input.GetButton("Fire2") && PlayerController.IsRotatingHeldItem)
            RotateHeldObject();
        if (Input.GetButtonUp("Fire2") && PlayerController.IsRotatingHeldItem)
            PlayerController.IsRotatingHeldItem = false;
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
            heldItem.transform.localRotation = Quaternion.identity;
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


    private void RotateHeldObject()
    {
        float y = Input.GetAxisRaw("Mouse X") * RotationSpeed;
        float x = -Input.GetAxisRaw("Mouse Y") * RotationSpeed;
        heldItem.transform.Rotate(0, y, 0);
        heldItem.transform.Rotate(x, 0, 0);
    }
}