using UnityEngine;

// Essa classe é usada para tocar audios diretamente na timeline de uma animacao

[RequireComponent(typeof(AudioSource))]
public class AudioPlay : MonoBehaviour
{
  AudioSource audioData;

  void Start()
  {
    audioData = GetComponent<AudioSource>();
  }

  public void PlayAudio( AudioClip au)
  {
    audioData.clip = au;
    audioData.Play();
  }
}