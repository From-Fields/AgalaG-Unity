using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private AudioSource _oneshotAudioSource;
    [Header("Sound Clips")][SerializeField]
    private AudioClip _damageSound;
    [SerializeField]
    private AudioClip _deathSound, _movementSound, _shotSound, _powerUpSound, _bounceSound;
    [Header("Prefabs")][SerializeField]
    private AudioSource _loopingAudioSourcePrefab;

    private Dictionary<EntityAudioType, AudioClip> _clips;
    private Dictionary<EntityAudioType, AudioSource> _loopingAudioSources;

    private void Awake() {
        _oneshotAudioSource = GetComponent<AudioSource>();

        _clips = new Dictionary<EntityAudioType, AudioClip>() {
            {EntityAudioType.Damage, _damageSound},
            {EntityAudioType.Death, _deathSound},
            {EntityAudioType.Movement, _movementSound},
            {EntityAudioType.Shot, _shotSound},
            {EntityAudioType.PowerUp, _powerUpSound},
            {EntityAudioType.Bounce, _bounceSound}
        };

        _loopingAudioSources = new Dictionary<EntityAudioType, AudioSource>();
    }

    public void PlaySound(AudioClip clip) => _oneshotAudioSource.PlayOneShot(clip);
    public void PlaySound(EntityAudioType audioType, bool looping = false) {
        AudioClip clip = _clips[audioType];

        if(clip == null)
            return;

        if(looping) {
            PlaySoundLooping(audioType, clip);
        } else {
            _oneshotAudioSource.clip = clip;
            _oneshotAudioSource.Play();
        }
    }
    private void PlaySoundLooping(EntityAudioType audioType, AudioClip clip) {
        AudioSource source;

        if(_loopingAudioSources.ContainsKey(audioType))
            StopSound(audioType);
        else {
            _loopingAudioSources.Add(
                audioType,
                Instantiate<AudioSource>(_loopingAudioSourcePrefab)
            );
            _loopingAudioSources[audioType].transform.SetParent(transform);
            _loopingAudioSources[audioType].transform.localPosition = Vector3.zero;
        }

        source = _loopingAudioSources[audioType];

        source.clip = clip;
        source.Play();
        source.loop = true;
    }

    public void StopSound(EntityAudioType audioType){
        if(!_loopingAudioSources.ContainsKey(audioType))
            return;

        AudioSource source = _loopingAudioSources[audioType];
        
        source.Stop();
        source.clip = null;        
    }

    public void WaitForAudioClipDone(System.Action callback) => StartCoroutine(AudioClipDoneCallback(callback));

    private IEnumerator AudioClipDoneCallback(System.Action callback) {
        while(_oneshotAudioSource.isPlaying) {
            yield return new WaitForEndOfFrame();
        }

        callback.Invoke();

        yield return null;
    }
}

public enum EntityAudioType {
    Damage,
    Death,
    Movement,
    Shot,
    PowerUp,
    Bounce
}
