using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixerGroup effectsAudioMixerGroup;
    public AudioMixerGroup musicAudioMixerGroup;

    private List<GameObject> audioSourcePool;
    private GameObject audioSourcePoolParent; // Declare a variable to store the pool GameObject
    private void Start()
    {
        audioSourcePool = new List<GameObject>();
    }

    private GameObject GetPooledObject()
    {
        if (audioSourcePoolParent == null)
        {
            audioSourcePoolParent = new GameObject("AudioSourcePool"); // Create a parent GameObject for the pool
        }

        foreach (Transform child in audioSourcePoolParent.transform)
        {
            if (!child.gameObject.activeInHierarchy)
            {
                child.gameObject.SetActive(true);
                return child.gameObject;
            }
        }

        var newGo = new GameObject("PlayAndForget");
        newGo.transform.parent = audioSourcePoolParent.transform; // Set the newGo as a child of the pool GameObject
        audioSourcePool.Add(newGo);
        return newGo;
    }

    public AudioSource PlayEffectClipAtPoint(AudioClip clip, Vector3 position, float volume = 1.0f, float pitch = 1f, float maxDist = 25f, float blend = 1f, float minDist = 10f)
    {
        var go = GetPooledObject();
        go.transform.position = position;

        var audioSource = go.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = go.AddComponent<AudioSource>();
        }
        audioSource.maxDistance = maxDist;
        audioSource.minDistance = minDist;
        audioSource.spatialBlend = blend;
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = effectsAudioMixerGroup;
        audioSource.volume = volume;
        audioSource.pitch = pitch;

        //Destroy(go, clip.length);
        StartCoroutine(ReturnToPool(clip.length, go));
        audioSource.Play();

        return audioSource;
    }

    public AudioSource PlayMusicClipAtPoint(AudioClip clip, Vector3 position, float volume = 1.0f, float pitch = 1f, float maxDist = 25f, float blend = 0f, bool looping = true)
    {
        var go = GetPooledObject();
        go.transform.position = position;

        var audioSource = go.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = go.AddComponent<AudioSource>();
        }
        audioSource.loop = looping;
        audioSource.maxDistance = maxDist;
        audioSource.spatialBlend = blend;
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = musicAudioMixerGroup;
        audioSource.volume = volume;
        audioSource.pitch = pitch;

        //Destroy(go, clip.length);
        StartCoroutine(ReturnToPool(clip.length, go));
        audioSource.Play();

        return audioSource;
    }

    IEnumerator ReturnToPool(float time, GameObject go)
    {
        yield return new WaitForSeconds(time);
        go.SetActive(false);

    }
}