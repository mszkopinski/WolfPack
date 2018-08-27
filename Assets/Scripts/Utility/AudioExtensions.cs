using UnityEngine;

namespace Wolfpack
{
    public static class AudioExtensions
    {
        public static AudioClip GetRandomClip(this AudioClip[] audioClips)
        {
            var rnd = Random.Range(0, audioClips.Length);
            return audioClips[rnd];
        }
    }
}