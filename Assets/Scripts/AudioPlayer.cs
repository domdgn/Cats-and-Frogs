using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Instance;
    public AudioClip gunFire, menuMusic, waitMusic, atkMusic, coinCollect, select;
    private AudioSource musicSource;
    private AudioSource sfxSource;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.parent.gameObject);

            AudioSource[] sources = GetComponents<AudioSource>();
            if (sources.Length >= 2)
            {
                musicSource = sources[0];
                sfxSource = sources[1];
            }
            else
            {
                musicSource = GetComponent<AudioSource>();
                sfxSource = gameObject.AddComponent<AudioSource>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SwitchMusic(menuMusic);
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void SwitchMusic(AudioClip newMusic)
    {
        musicSource.Stop();
        musicSource.clip = newMusic;
        musicSource.Play();
    }
}
