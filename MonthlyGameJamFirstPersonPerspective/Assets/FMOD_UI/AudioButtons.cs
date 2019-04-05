using UnityEngine;
using UnityEngine.UI;

public class AudioButtons : MonoBehaviour
{
    public Button closeButton;
    
    private void Start()
    {
        closeButton?.GetComponent<Button>().onClick.AddListener(ToggleActive);
    }

    public void ToggleActive()
    {
        GetComponentInParent<AudioMaster>().ToggleActive();
    }
}