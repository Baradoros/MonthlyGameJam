using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioMaster : MonoBehaviour
{
    public static AudioMaster instance;

    [Tooltip("Assign FMOD Canvas if you want to use it")]
    public bool settingsStartActive;
    public GameObject settingsUI;
    
    [Space(10)]
    [SerializeField, EventRef] private string testSFX = "";
    [SerializeField] private EventInstance SFXVolumeTestEvent;

    [Space(10)]
    [Range(0,1)] public float mainVolume = 1f;
    [Range(0,1)] public float musicVolume = 1f;
    [Range(0,1)] public float sfxVolume = 1f;
    
    private Bus mainBus;
    private Bus musicBus;
    private Bus sfxBus;
    private const string mainVolumePrefString = "Main_Volume";
    private const string musicVolumePrefString = "Music_Volume";
    private const string sfxVolumePrefString = "SFX_Volume";
    private float oldMainVolume = 1f; // for testing Main
    private float oldSfxVolume = 1f; // for testing SFX

    private void OnEnable()
    {
        Singleton();
        
        if (settingsUI != null)
        {
            settingsUI.gameObject.SetActive(settingsStartActive);
        }
    }

    private void Start()
    {       
        // Create an instance of the sound to be played
        SFXVolumeTestEvent = RuntimeManager.CreateInstance(testSFX);
        
        // Establish the buses to be loaded and managed
        mainBus = RuntimeManager.GetBus("bus:/Main");
        musicBus = RuntimeManager.GetBus("bus:/Main/Music");
        sfxBus = RuntimeManager.GetBus("bus:/Main/SFX");  
        
        // Check player prefs, if there is a value for each take it, if not make it
        if (PlayerPrefs.HasKey(mainVolumePrefString)) 
            mainVolume = PlayerPrefs.GetFloat(mainVolumePrefString);
        
        if (PlayerPrefs.HasKey(musicVolumePrefString))
            musicVolume = PlayerPrefs.GetFloat(musicVolumePrefString);
        
        if (PlayerPrefs.HasKey(sfxVolumePrefString))
            sfxVolume = PlayerPrefs.GetFloat(sfxVolumePrefString);

        // reset the oldValues
        oldMainVolume = mainVolume;
        oldSfxVolume = sfxVolume;
    }

    private void Update()
    {        
        // Listen for changes in the audio menu
        SetMasterVolume(mainVolume);
        SetMusicVolume(musicVolume);
        SetSfxVolume(sfxVolume);

        if (!settingsUI) return;
            
        UsingThisUI();
    }

    public void SetMasterVolume(float value)
    {
        mainVolume = value;
        mainBus.setVolume(mainVolume);
        PlayerPrefs.SetFloat(mainVolumePrefString, mainVolume);
        
        SFXPlaySound(ref oldMainVolume, mainVolume);
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        musicBus.setVolume(musicVolume);
        PlayerPrefs.SetFloat(musicVolumePrefString, musicVolume);
    }
    
    public void SetSfxVolume(float value)
    {
        sfxVolume = value;
        
        sfxBus.setVolume(sfxVolume);
        PlayerPrefs.SetFloat(sfxVolumePrefString, sfxVolume);
        
        SFXPlaySound(ref oldSfxVolume, sfxVolume);
    } 

    private void SFXPlaySound(ref float oldVolume, float newVolume)
    {
        if (oldVolume == newVolume) return;
        
        // Play a sound when during changes
        PLAYBACK_STATE playbackState;
        SFXVolumeTestEvent.getPlaybackState(out playbackState);
        if (playbackState != PLAYBACK_STATE.PLAYING)
        {
            SFXVolumeTestEvent.start();
        }

        oldVolume = newVolume;
    }

    private void UsingThisUI()
    {        
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleActive();
        }
    }

    public void ToggleActive()
    {
        settingsUI.SetActive(!settingsUI.activeSelf);
    }
    
    private void Singleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Debug.Log("not the instance " + name);
            Destroy(gameObject);
        }
    }
}