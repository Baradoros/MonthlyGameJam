using UnityEngine;

public class BasicCubeInteractable : MonoBehaviour, IInteractable
{
    private Transform holderT;
    private HoldableObject holdableObject; 

    public int testNum;
    
    private void Awake()
    {
        holderT = GameObject.FindWithTag("Holder").transform;
    }

    public bool Holdable()
    {
        return false;
    }

    public bool Actionable()
    {
        return true;
    }

    public void Interact()
    {
        Debug.Log("This Cube is Interactable");
        CheckWhatPlayerIsHolding();
    }

    public void CheckWhatPlayerIsHolding()
    {
        // make sure the player is holding something
        if (holderT.childCount > 0)
        {
            holdableObject = holderT.GetChild(0).gameObject.GetComponent<HoldableObject>();
            
            // determine success or fail, by passing this component to the holdableObject for compare
            if (holdableObject.HoldableInteractionTest(this))
            {
                Debug.Log("You have the right cube");
                holdableObject.DropObject();
                holdableObject.transform.position = transform.position + Vector3.up;
            }
            else
            {
                Debug.Log("This is the wrong cube");
            }
        }
        else
        {
            Debug.Log("You need to hold the right cube to use this cube");
        }
    }
}