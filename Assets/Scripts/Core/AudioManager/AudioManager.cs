using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource m_musicAudioSource;
    [SerializeField] private AudioSource m_SFXAudioSource;
    [SerializeField] private AudioSource m_narrationAudioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip m_buttonClickSFX;

    private float m_musicVolume = 1f;
    private float m_sfxVolume = 1f;
    private float m_narrationVolume = 1f;

    private bool m_isMuted = false;

    //=============================
    // PLAY METHODS
    //=============================
    public void StopAll()
    {
        m_musicAudioSource.Stop();
        m_SFXAudioSource.Stop();
        m_narrationAudioSource.Stop();
    }
    public void PlayMusic(AudioClip clip)
    {
        m_musicAudioSource.Stop();

        if (clip == null)
        {
            Debug.LogWarning("[AudioManager] Music clip is null!");
            return;
        }

        m_musicAudioSource.clip = clip;
        m_musicAudioSource.volume = m_musicVolume;
        m_musicAudioSource.loop = true;
        m_musicAudioSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        m_SFXAudioSource.Stop();

        if (clip == null)
        {
            Debug.LogWarning("[AudioManager] SFX clip is null!");
            return;
        }

        m_SFXAudioSource.clip = clip;
        m_SFXAudioSource.volume = m_sfxVolume;
        m_SFXAudioSource.loop = false;
        m_SFXAudioSource.Play();
    }

    public void PlayNarration(AudioClip clip)
    {
        m_narrationAudioSource.Stop();

        if (clip == null)
        {
            Debug.LogWarning("[AudioManager] Narration clip is null!");
            return;
        }

        m_narrationAudioSource.clip = clip;
        m_narrationAudioSource.volume = m_narrationVolume;
        m_narrationAudioSource.loop = false;
        m_narrationAudioSource.Play();
    }

    //=============================
    // VOLUME CONTROLS
    //=============================

    public void NarrationVolume(float volume)
    {
        m_narrationVolume = Mathf.Clamp01(volume);
        m_narrationAudioSource.volume = m_isMuted ? 0f : m_narrationVolume;
    }

    public void SFXVolume(float volume)
    {
        m_sfxVolume = Mathf.Clamp01(volume);
        m_SFXAudioSource.volume = m_isMuted ? 0f : m_sfxVolume;
    }

    public void MusicVolume(float volume)
    {
        m_musicVolume = Mathf.Clamp01(volume);
        m_musicAudioSource.volume = m_isMuted ? 0f : m_musicVolume;
    }

    //=============================
    // MUTE TOGGLE
    //=============================

    public void MuteToggle(bool mute)
    {
        m_isMuted = mute;

        //float targetVolume = mute ? 0f : 1f;

        m_musicAudioSource.volume = mute ? 0f : m_musicVolume;
        m_SFXAudioSource.volume = mute ? 0f : m_sfxVolume;
        m_narrationAudioSource.volume = mute ? 0f : m_narrationVolume;
    }

    //=============================
    // QUICK ACCESS
    //=============================

    public void PlaySFXButtonClick()
    {
        PlaySFX(m_buttonClickSFX);
    }
}
