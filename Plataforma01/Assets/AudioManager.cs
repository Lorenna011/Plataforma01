using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private AudioSource systemSource;
    private List<AudioSource> activeSource;

    #region Singleton

    public static AudioManager Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            systemSource = GetComponent<AudioSource>();
            activeSource = new List<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }

    }

    #endregion

    #region AudioControls

    public void Play(AudioClip clip)
    {
        systemSource.Stop();
        systemSource.clip = clip;
        systemSource.Play();
    }

    public void Stop()
    {
        systemSource.Stop();
    }

    public void Pause()
    {
        systemSource.Pause();
    }

    public void Resume()
    {
        systemSource.UnPause();
    }
    
    public void Play(AudioClip clip, AudioSource source)
    {
        if(activeSource.Contains(source))
            activeSource.Add(source);
        source.Stop();
        source.clip = clip;
        source.Play();
    }

    public void Stop(AudioSource source)
    {
        if(activeSource.Contains(source))
            systemSource.Stop();
        activeSource.Remove(source);
    }

    public void Pause()
    {
        if(activeSource.Contains(source))
            source.Pause()
    }

    public void Resume()
    {
        if(activeSource.Contains(source))
            source.UnPause();
    }


    

}
#endregion
}
