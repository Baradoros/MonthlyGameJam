using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public GameObject currentActionableObject; // action dot is currently over this object
    private Transform holderT; // transform that objects are held in (parent for them)

    private void Awake()
    {
        holderT = GameObject.FindWithTag("Holder").transform;
    }

    private void Update()
    {
        CurrentTargetedObject();
        ExecuteInteraction();
    }

    private bool CurrentTargetedObject()
    {
        // from camera to the center of the screen based on where we are 'looking'
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // store what the raycast hits
        RaycastHit hit;
        
        // get object where we are 'looking'
        if (Physics.Raycast(ray, out hit))
        {
            var selectable = hit.transform.GetComponent<IInteractable>();
            
            if (selectable is IInteractable) 
            {
                // sets property in this script
                currentActionableObject = hit.transform.gameObject;

                return true;
            }
        }
        
        // nulls property in this script
        currentActionableObject = null;
        return false;
    }
    
    private void ExecuteInteraction()
    {
        if (Input.GetButtonDown("Fire1")) 
        {
            Debug.Log("Fire1");
            
            if (currentActionableObject != null) 
            {
                currentActionableObject.GetComponent<IInteractable>()?.Interact();
                currentActionableObject.GetComponent<HoldableObject>()?.PickUpObject();
            }
            else
            {
                // can drop heldObject by clicking any open place
                if (holderT.childCount > 0)
                    holderT.GetChild(0).GetComponent<HoldableObject>()?.DropObject();
            }
        }
    }
    
    /// <summary>
    /// Compare to a layer, to an inspector set Layermask which may contain multiple layers
    /// </summary>
    /// <param name="layer">player - layer to get return value for</param>
    /// <param name="layermask">LayerMask - layers to compare against</param>
    /// <returns>Returns true if a layer in included in a set of layers.</returns>
    public static bool IsInLayerMask(int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }
}