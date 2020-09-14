using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get { return instance; } }
    private static SoundManager instance;

    [SerializeField]
    AudioSource BGAudioSource;

    [SerializeField]
    AudioSource recordingTipAudioSource;

    AudioSource audioSource;
    bool IsMuted = false;

    [SerializeField]
    AudioClip ClipTracking;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.audioSource = GetComponent<AudioSource>();
    }

    public void PlayTrackingFound()
    {

        this.audioSource.PlayOneShot(ClipTracking);

    }
    public void PlayClip(AudioClip clip)
    {

        this.audioSource.PlayOneShot(clip);

    }
    public void PlayRecordingTip(AudioClip clip)
    {

        recordingTipAudioSource.clip = clip;
        recordingTipAudioSource.Play();

    }
    public void SetSound(bool activate)
    {
        this.IsMuted = activate;
        if (!this.IsMuted)
        {
            this.BGAudioSource.Play();
        }
        else
        {
            this.BGAudioSource.Stop();
        }
    }

    public void Stop()
    {
        this.audioSource.Stop();
    }

    public void disableMusic()
    {
        if (BGAudioSource.isPlaying)
            BGAudioSource.Pause();
    }

    public void enableMusic()
    {
        if (!this.IsMuted)
            if (!BGAudioSource.isPlaying)
                BGAudioSource.Play();
    }

    public void PauseRecordingTip()
    {
        if (recordingTipAudioSource.isPlaying)
            recordingTipAudioSource.Pause();
    }

    void Update()
    {
        BGAudioSource.volume = recordingTipAudioSource.isPlaying ? 0 : 0.151f;
    }

    public AudioClip GetRecordingTipAudioClip()
    {
        return recordingTipAudioSource.clip;
    }

    public bool IsPlayingRecordingTip()
    {
        return recordingTipAudioSource.isPlaying;
    }

}
