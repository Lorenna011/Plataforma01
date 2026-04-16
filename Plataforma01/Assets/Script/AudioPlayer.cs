using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ClipEntry
{
    public AudioClip clip;
    [Tooltip("Index into the AudioSources array. If out of range, the AudioManager's system source will be used.")]
    public int audioSourceIndex = -1;
}

public class AudioPlayer : MonoBehaviour
{
    [Tooltip("List of clips with optional audio source index to play each clip on.")]
    public List<ClipEntry> clips = new List<ClipEntry>();

    [Tooltip("AudioSources that can be referenced by index from the clips list.")]
    public AudioSource[] audioSources;

    [Tooltip("Selected clip index used by the editor controls and runtime helper methods.")]
    public int selectedIndex;

    /// <summary>
    /// Play the clip at the given index using AudioManager. If the clip has a valid audioSourceIndex and
    /// the corresponding AudioSource exists, the AudioManager.Play(clip, source) overload is used. Otherwise
    /// the AudioManager.Play(clip) system source is used.
    /// </summary>
    public void PlayClip(int index)
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogWarning("AudioManager.Instance is null. Make sure an AudioManager exists in the scene.");
            return;
        }

        if (index < 0 || index >= clips.Count)
        {
            Debug.LogWarning($"PlayClip: index {index} is out of range (0..{Mathf.Max(0, clips.Count - 1)}).");
            return;
        }

        var entry = clips[index];
        if (entry == null || entry.clip == null)
        {
            Debug.LogWarning($"PlayClip: no clip assigned at index {index}.");
            return;
        }

        AudioSource src = GetSourceForEntry(entry);
        if (src != null)
            AudioManager.Instance.Play(entry.clip, src);
        else
            AudioManager.Instance.Play(entry.clip);
    }

    public void StopClip(int index)
    {
        if (AudioManager.Instance == null) return;
        if (index < 0 || index >= clips.Count) return;
        var entry = clips[index];
        AudioSource src = GetSourceForEntry(entry);
        if (src != null)
            AudioManager.Instance.Stop(src);
        else
            AudioManager.Instance.Stop();
    }

    public void PauseClip(int index)
    {
        if (AudioManager.Instance == null) return;
        if (index < 0 || index >= clips.Count) return;
        var entry = clips[index];
        AudioSource src = GetSourceForEntry(entry);
        if (src != null)
            AudioManager.Instance.Pause(src);
        else
            AudioManager.Instance.Pause();
    }

    public void ResumeClip(int index)
    {
        if (AudioManager.Instance == null) return;
        if (index < 0 || index >= clips.Count) return;
        var entry = clips[index];
        AudioSource src = GetSourceForEntry(entry);
        if (src != null)
            AudioManager.Instance.Resume(src);
        else
            AudioManager.Instance.Resume();
    }

    // Helper used by the editor and runtime helpers
    private AudioSource GetSourceForEntry(ClipEntry entry)
    {
        if (entry == null) return null;
        int idx = entry.audioSourceIndex;
        if (audioSources == null) return null;
        if (idx >= 0 && idx < audioSources.Length)
            return audioSources[idx];
        return null;
    }

    // Convenience methods operating on the currently selected index
    public void PlaySelected() => PlayClip(selectedIndex);
    public void StopSelected() => StopClip(selectedIndex);
    public void PauseSelected() => PauseClip(selectedIndex);
    public void ResumeSelected() => ResumeClip(selectedIndex);
}