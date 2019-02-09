using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [Tooltip("")] public AudioMixer audioMixer;
    [Tooltip("")] public string masterVolumeExposedParameterName;
    [Tooltip("")] public string musicVolumeExposedParameterName;
    [Tooltip("")] public string sfxVolumeExposedParameterName;
    [SerializeField] [Tooltip("")] private AudioSource menuMusic;
    [SerializeField] [Tooltip("")] private AudioSource playMusic;

    public enum MusicType { NONE, MENU, PLAY }
    public MusicType musicType { get; private set; }

    private Dictionary<MusicType, AudioSource> sources;

    public static AudioManager _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }

        sources = new Dictionary<MusicType, AudioSource>();

        foreach (MusicType pieceType in Enum.GetValues(typeof(MusicType)))
        {
            sources[pieceType] = new AudioSource();
        }

        foreach (string exposedParameterName in new string[]{masterVolumeExposedParameterName, musicVolumeExposedParameterName, sfxVolumeExposedParameterName})
        {
            audioMixer.SetFloat(exposedParameterName, PlayerPrefs.GetFloat(exposedParameterName));
        }

        sources[MusicType.MENU] = menuMusic;
        sources[MusicType.PLAY] = playMusic;
    }

    public void SetMusicType(MusicType type)
    {
        if (type == MusicType.NONE)
        {
            StopSounds();
            return;
        }

        StopSounds();
        PlaySound(type);

        musicType = type;
    }

    public void StopSounds()
    {
        foreach (KeyValuePair<MusicType, AudioSource> pair in sources)
        {
            AudioSource source = pair.Value;

            if (source != null && source.isPlaying)
            {
                source.Stop();
            }
        }
    }

    public void PlaySound(MusicType type)
    {
        AudioSource source = sources[type];

        if (source != null)
        {
            source.enabled = true;
            source.Play();
        }
    }

    public void SetMasterGroupVolume(float volumeLevel)
    {
        SetGroupVolume(masterVolumeExposedParameterName, volumeLevel);
    }
    public void SetMusicGroupVolume(float volumeLevel)
    {
        SetGroupVolume(musicVolumeExposedParameterName, volumeLevel);
    }

    public void SetSFXGroupVolume(float volumeLevel)
    {
        SetGroupVolume(sfxVolumeExposedParameterName, volumeLevel);
    }

    private void SetGroupVolume(string exposedParameterName, float volumeLevel)
    {
        audioMixer.SetFloat(exposedParameterName, volumeLevel);
        PlayerPrefs.SetFloat(exposedParameterName, volumeLevel);
        PlayerPrefs.Save();
    }
}