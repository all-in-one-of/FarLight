using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace fl
{
    /// <summary>
    ///  Данный класс отвечает за все воспроизводимые звуки.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClip[] audioClip;
        private Dictionary<string, AudioSource> audioSource;

        public void Play(string name)
        {
            audioSource[name].Play();
        }

        private static AudioManager Instance;
        private void Awake()
        {
            Instance = this;

            audioSource = new Dictionary<string, AudioSource>();

            foreach(var clip in Instance.audioClip)
            {
                audioSource[clip.name] = gameObject.AddComponent<AudioSource>();
                audioSource[clip.name].playOnAwake = false;
                audioSource[clip.name].clip = clip;
            }
        }
        public static AudioManager GetInstance()
        {
            return Instance;
        }
    }
}

