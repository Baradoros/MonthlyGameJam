using System;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private LayerMask selectableLayers; // layers an object is on, that make it selectable
    [SerializeField] private Material highlightedMaterial; // material to place on selectable object

    [Header("Selection Variables")]
    public float SelectionRange;
    public GameObject currentSelection;
    [SerializeField] private Material originalMaterial; // store original material so we can return it when not selected

    // Event to notify listeners that there is a selectable object, so they can do something with that information as needed
    public static event Action<GameObject> OnObjectSelected = delegate {};
    
    private void Update()
    {
        // when an object is not selectale (e.g. not on our raycast/red-dot return it's material
        // @TODO we may want to just change the UI (e.g. make the red dot green?)
        if (currentSelection != null)
        {
            Renderer selectionRenderer = currentSelection.GetComponent<Renderer>();
            if (selectionRenderer == null) return;
            
            selectionRenderer.material = originalMaterial;
            currentSelection = null;
        }

        // using raycast, we will get the screen point center
        CurrentTargetedObject();
        
        // send event for other scripts that may want to know what is selected (e.g. in hand)
        OnObjectSelected(currentSelection);
    }

    private void CurrentTargetedObject()
    {
        // from camera to the center of the screen based on where we are 'looking'
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // store what the raycast hits
        RaycastHit hit;
        
        // get object where we are 'looking'
        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform.gameObject;

            // check to see if the hit object is on a selectableLayer and within range
            if (IsInLayerMask(selection.layer, selectableLayers) && Vector3.Distance(transform.position, hit.transform.position) < SelectionRange)
            {
                
                // identifying this is selectable
                // @TODO as mentioned above, we may want this to change the UI cursor/indicator
                //    and not the actual object.
                Renderer selectionRenderer = selection.GetComponent<Renderer>();

                if (selectionRenderer != null)
                {
                    // store the original material
                    originalMaterial = selectionRenderer.material;
                    
                    // use our designated highlighted material.
                    selectionRenderer.material = highlightedMaterial;
                }

                // sets object in this script
                currentSelection = selection;
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