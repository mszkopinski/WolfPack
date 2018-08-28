using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace Wolfpack
{
    public static class AudioExtensions
    {
        public static AudioClip GetRandomClip(this AudioClip[] audioClips)
        {
            var rnd = Random.Range(0, audioClips.Length);
            return audioClips[rnd];
        }

        public static void SetVolume(this AudioMixer audioMixer, string parameterName, float volume)
        {
            volume = (1 - Mathf.Sqrt(volume)) * -80f;
            audioMixer.SetFloat(parameterName, volume);
        }
        
        public static IEnumerator ChangeVolumeOverTime(this AudioMixer audioMixer, string parameterName, float toValue,
            float timeInS)
        {
            var elapsedTime = 0f;
            
            while (elapsedTime < timeInS)
            {
                elapsedTime += Time.deltaTime;
                float currentValue = audioMixer.GetVolume01(parameterName);
                audioMixer.SetVolume(parameterName, Mathf.Lerp(currentValue, toValue, elapsedTime / timeInS));
                yield return null;
            }
        }
        
        public static IEnumerator ChangeVolumeOverTimeWithDelay(this AudioMixer audioMixer, string parameterName, float toValue,
            float timeInS, float delayInS)
        {
            var elapsedTime = 0f;
            yield return new WaitForSeconds(delayInS);
            
            while (elapsedTime < timeInS)
            {
                elapsedTime += Time.deltaTime;
                float currentValue = audioMixer.GetVolume01(parameterName);
                audioMixer.SetVolume(parameterName, Mathf.Lerp(currentValue, toValue, elapsedTime / timeInS));
                yield return null;
            }
        }

        public static IEnumerator ChangeFloatOverTime(this AudioMixer audioMixer, string parameterName, float toValue,
            float timeInS)
        {
            var elapsedTime = 0f;
            
            while (elapsedTime < timeInS)
            {
                elapsedTime += Time.deltaTime;
                float currentValue;
                audioMixer.GetFloat(parameterName, out currentValue);
                audioMixer.SetFloat(parameterName, Mathf.Lerp(currentValue, toValue, elapsedTime / timeInS));
                yield return null;
            }
        }
        
        public static IEnumerator ChangeFloatOverTimeWithDelay(this AudioMixer audioMixer, string parameterName, float toValue,
            float timeInS, float delayInS)
        {
            var elapsedTime = 0f;
            yield return new WaitForSeconds(delayInS);
            
            while (elapsedTime < timeInS)
            {
                elapsedTime += Time.deltaTime;
                float currentValue;
                audioMixer.GetFloat(parameterName, out currentValue);
                audioMixer.SetFloat(parameterName, Mathf.Lerp(currentValue, toValue, elapsedTime / timeInS));
                yield return null;
            }
        }

        public static float GetVolume01(this AudioMixer audioMixer, string parameterName)
        {
            float currentVolume;
            audioMixer.GetFloat(parameterName, out currentVolume);
            return Mathf.Pow(-(currentVolume / -80f - 1f), 2f);
        }


        public static void ChangeClip(this AudioSource audioSource, AudioClip audioClip)
        {
            audioSource.Stop();
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        public static IEnumerator ChangeClipWithDelay(this AudioSource audioSource, AudioClip audioClip, float delayInS, [CanBeNull] Action callback)
        {
            yield return new WaitForSeconds(delayInS);
            audioSource.ChangeClip(audioClip);
            callback?.Invoke();
        }
        
    }
}