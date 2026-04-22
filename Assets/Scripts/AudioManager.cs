using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.PlayerLoop;

public class AudioManager : MonoBehaviour{

    // a class to store most of the sounds that doesn't require any 3d sound.
    [Serializable]
    public class Sounds {
        public AudioClip clip;
        public string name;
        public float volume;
        public float pitch;
        public bool loop;
        public AudioSource source;
        public float baseVolume;
    }
    // list of all sounds(audio sources in this game object). 
    public Sounds[] sounds;

    // list of all audio sources in this game object and other game objects.
    private List<AudioSource> allAudioSources = new List<AudioSource>();
    
    // list of all base volumes for external audio sources.
    private Dictionary<AudioSource, float> externalBaseVolumes = new Dictionary<AudioSource, float>();

    public float globalPitch = 1f;
    
    public static AudioManager instance;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
        
        // setting up all the sounds in the object so that they have an audio source
        foreach (Sounds s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            
            // storing original volume 
            s.baseVolume = s.volume;
            
            // registering the list of sounds in the game object to allAudioSources.
            RegisterAudioSource(s.source);
        }
    }

    private void Start() {
        UpdateVolume();
    }

    private void Update() {
        globalPitch = Time.timeScale;
        UpdateAllAudioSources();
    }

    // single audio source control functions
    public void Play(string soundName) {
        Sounds s = Array.Find(sounds, sound => sound.name == soundName);
        if (s != null) {
            s.source.Play();
        }
    }
    
    public void Stop(string soundName) {
        Sounds s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null) {
            Debug.LogWarning("Sound " + soundName + " not found!");
            return;
        }
        s.source.Stop();
    }
    
    public void FadeOut(string soundName)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == soundName);
        if (s != null)
        {
            StartCoroutine(FadeOutCoroutine(s));
        }
    }

    private IEnumerator FadeOutCoroutine(Sounds s)
    {
        float startVolume = s.source.volume;
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.unscaledDeltaTime;
            s.source.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / 1f);
            yield return null;
        }
        s.source.Stop();
        s.source.volume = startVolume;
    }
    
    // multi audio sources control functions
    public void StopAll()
    {
        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource != null)
                audioSource.Stop();
        }
    }
    
    public void PauseAll()
    {
        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource != null)
                audioSource.Pause();
        }
    }

    public void ResumeAll()
    {
        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource != null)
                audioSource.UnPause();
        }
    }
    
    public void UpdateVolume()
    {
        float masterVolume = PlayerPrefs.GetFloat("Master Volume", 1f);
        float musicVolume = PlayerPrefs.GetFloat("Music Volume", 1f);

        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource != null)
            {
                // Check if the audio source is internal (part of Sounds array)
                Sounds internalSound = Array.Find(sounds, s => s.source == audioSource);

                if (internalSound != null)
                {
                    if (internalSound.name == "Music")
                    {
                        // Music: Apply both master and music volume
                        audioSource.volume = internalSound.baseVolume * masterVolume * musicVolume;
                    }
                    else
                    {
                        // Internal non-music sounds: Apply master volume
                        audioSource.volume = internalSound.baseVolume * masterVolume;
                    }
                }
                else if (externalBaseVolumes.ContainsKey(audioSource))
                {
                    // External source: Scale its base volume by master volume
                    audioSource.volume = externalBaseVolumes[audioSource] * masterVolume;
                }
            }
        }
    }

    
    // registering audio sources when a new audio source is added 
    public void RegisterAudioSource(AudioSource audioSource)
    {
        if (!allAudioSources.Contains(audioSource))
        {
            allAudioSources.Add(audioSource);

            // saving external audio sources' base volumes.
            bool isInternal = Array.Exists(sounds, sound => sound.source == audioSource);
            if (!isInternal) {
                if (!externalBaseVolumes.ContainsKey(audioSource))
                    externalBaseVolumes[audioSource] = audioSource.volume;
            }
        }
        UpdateAudioSource(audioSource);
        UpdateVolume();
    }
    
    // unregistering when the game object of the audio source is destroyed
    public void UnregisterAudioSource(AudioSource audioSource) {
        if (allAudioSources.Contains(audioSource)) {
            allAudioSources.Remove(audioSource);
            
            // removing the base volume from the dictionary as well.
            if (externalBaseVolumes.ContainsKey(audioSource))
                externalBaseVolumes.Remove(audioSource);
        }
    }

    private void UpdateAudioSource(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            audioSource.pitch = globalPitch;
        }
    }
    
    private void UpdateAllAudioSources()
    {
        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource != null)
            {
                UpdateAudioSource(audioSource);
            }
        }
    }
}
