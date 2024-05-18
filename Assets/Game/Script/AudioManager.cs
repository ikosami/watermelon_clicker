using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioClip[] audios;

    /// <summary>
    /// 開始処理
    /// </summary>
    void Awake()
    {
        instance = this;
    }

    public void PlaySE(int se)
    {
        var audio = gameObject.AddComponent<AudioSource>();
        audio.clip = audios[se];
        audio.volume = 0.5f;
        audio.Play();

        StartCoroutine(Remove(audio));
    }

    IEnumerator Remove(AudioSource audio)
    {
        yield return new WaitForSeconds(5);
        Destroy(audio);

    }
}
