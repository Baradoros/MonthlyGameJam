using UnityEngine;
using UnityEngine.UI;

public class AudioSliders : MonoBehaviour
{
    #pragma warning disable    
    [Header("Sliders")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    #pragma warning enable

    private AudioMaster audioMaster;

    private void Start()
    {
        SetUpAudioMaster();
        
        // Set Up Controls
        masterVolumeSlider.value = AudioMaster.instance.mainVolume;
        masterVolumeSlider.onValueChanged.AddListener(AudioMaster.instance.SetMasterVolume);

        musicVolumeSlider.value = AudioMaster.instance.musicVolume;
        musicVolumeSlider.onValueChanged.AddListener(AudioMaster.instance.SetMusicVolume);

        sfxVolumeSlider.value = AudioMaster.instance.sfxVolume;
        sfxVolumeSlider.onValueChanged.AddListener(AudioMaster.instance.SetSfxVolume);
    }

    private void SetUpAudioMaster()
    {
        if (AudioMaster.instance == null) // might need script order adjustment
        {
            Debug.Log("There is no AudioMaster to control");
        }
    }
}