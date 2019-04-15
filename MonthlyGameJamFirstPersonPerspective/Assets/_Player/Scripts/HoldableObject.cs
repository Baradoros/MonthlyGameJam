using UnityEngine;

public abstract class HoldableObject : MonoBehaviour
{    
    private Transform holderT;

    private void Awake()
    {
        holderT = GameObject.FindWithTag("Holder").transform;
    }

    public void PickUpObject()
    {
        // drop any held object before picking up new one
        if (holderT.childCount > 0)
            holderT.GetChild(0).GetComponent<HoldableObject>().DropObject();
        
        // set to Kinematic so it doesn't keep falling or getting on our way
        if (GetComponent<Rigidbody>() != null) 
            GetComponent<Rigidbody>().isKinematic = true;
            
        // position so that it seems like we are 'holding' it
        transform.SetParent(holderT);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0,0,0);
    }

    public void DropObject()
    {
        // reset the rb Kinematics
        if (GetComponent<Rigidbody>() != null) 
            GetComponent<Rigidbody>().isKinematic = false;

        // postion it back in the world like we 'dropped' it in front of ourselves
        transform.SetParent(null);
        transform.localPosition =
            holderT.GetComponentInParent<PlayerController>().transform.position + transform.forward;
    }

    /// <summary>
    /// To be overwritten in the class this is used, to determine success/fail when player
    /// clicks on a IInteractable, while holding the object this class is inherits
    /// </summary>
    /// <param name="testAgainst">The component of the IInteractable being clicked</param>
    /// <returns>Returns bool based on developers code.</returns>
    public virtual bool HoldableInteractionTest(Component testAgainst)
    {
        Debug.Log(testAgainst);
        Debug.Log("Interaction Checking Needed for " + name, gameObject);
        return false;
    }
}