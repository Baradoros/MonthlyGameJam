using System;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private LayerMask selectableLayers;
    [SerializeField] private Material highlightedMaterial;

    public GameObject currentSelection;
    [SerializeField] private Material originalMaterial;

    public static event Action<GameObject> OnObjectSelected = delegate {};
    
    private void Update()
    {
        if (currentSelection != null)
        {
            Renderer selectionRenderer = currentSelection.GetComponent<Renderer>();
            if (selectionRenderer == null) return;
            
            selectionRenderer.material = originalMaterial;
            currentSelection = null;
        }

        CurrentTargetedObject();
        
        OnObjectSelected(currentSelection);
    }

    private void CurrentTargetedObject()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform.gameObject;

            if (IsInLayerMask(selection.layer, selectableLayers))
            {
                Renderer selectionRenderer = selection.GetComponent<Renderer>();

                if (selectionRenderer != null)
                {
                    originalMaterial = selectionRenderer.material;
                    selectionRenderer.material = highlightedMaterial;
                }

                currentSelection = selection;
            }
        } 
    }
    
    public static bool IsInLayerMask(int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }
}