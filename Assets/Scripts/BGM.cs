using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip clip;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (clip != null)
            StartCoroutine(FadeInVolume(clip));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartFadeIn(AudioClip audioClip)
    {
        StartCoroutine(FadeInVolume(audioClip));
    }

    IEnumerator FadeInVolume(AudioClip audioClip)
    {
        float currentTime = 0;

        audioSource.clip = audioClip;
        audioSource.Play();

        while (currentTime < 3)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, 0.2f, currentTime / 3);
            yield return null;
        }
    }

    public void FadeOutVolume()
    {

    }
}
