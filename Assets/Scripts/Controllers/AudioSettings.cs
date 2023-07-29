using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : SingletonMonoBehaviour<AudioSettings>
{
    public AudioMixer _mainMixer;

    private const string MasterVolume = "MasterVolume";
    private const string MusicVolume = "BGMVolume";
    private const string SFXVolume = "SFXVolume";


    /// <summary>
    /// Apply master volume beetween 1 and 100
    /// </summary>
    public void ApplyMasterVolume(int value)
    {
        ApplyMixerVolume(MasterVolume, value);
    }

    /// <summary>
    /// Apply master volume beetween 0.0001f and 1f
    /// </summary>
    public void ApplyMasterVolume(float value)
    {
        ApplyMixerVolume(MasterVolume, value);
    }

    /// <summary>
    /// Apply music volume beetween 1 and 100
    /// </summary>
    public void ApplyMusicVolume(int value)
    {
        ApplyMixerVolume(MusicVolume, value);
    }

    /// <summary>
    /// Apply music volume beetween 0.0001f and 1f
    /// </summary>
    public void ApplyMusicVolume(float value)
    {
        ApplyMixerVolume(MusicVolume, value);
    }

    /// <summary>
    /// Apply sound effect volume beetween 1 and 100
    /// </summary>
    public void ApplySFXVolume(int value)
    {
        ApplyMixerVolume(SFXVolume, value);
    }

    /// <summary>
    /// Apply sound effect volume beetween 0.0001f and 1f
    /// </summary>
    public void ApplySFXVolume(float value)
    {
        ApplyMixerVolume(SFXVolume, value);
    }

    private void ApplyMixerVolume(string key, float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        _mainMixer.SetFloat(key, Mathf.Log10(value) * 20);
    }

    private void ApplyMixerVolume(string key, int value)
    {
        value = Mathf.Clamp(value, 1, 100);
        ApplyMixerVolume(key, value / 100f);
    }
}