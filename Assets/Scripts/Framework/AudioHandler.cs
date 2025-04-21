using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tp2
{
    [System.Serializable]
    public class Clip
    {
        [Tooltip("AudioClip that should be played")]
        public AudioClip clip;
        [Tooltip("Timeout for the clip. If exceeded, then drop")]
        public float timeout;
        public float volume;
        public float pitch;
        public float startTime;
        public float variance;
        float time;

        /// <summary>
        /// Creates a new Clip
        /// </summary>
        /// <param name="clip">The Audioclip to be played</param>
        /// <param name="volume">The volume to play the clip at</param>
        /// <param name="pitch">The pitch to play the clip at</param>
        /// <param name="timeout">Won't play if it takes more than x seconds. -1 to disable</param>
        /// <param name="sTime">Time in seconds to start the AudioClip at</param>
        public Clip(AudioClip clip, float volume = 1, float pitch = 1, float timeout = 10, float sTime = 0, float variance = .2f)
        {
            this.clip = clip;
            this.timeout = timeout;
            this.volume = volume;
            this.pitch = pitch;
            this.variance = variance;
            startTime = sTime;
            time = Time.time;
        }
        /// <summary>
        /// Fetches if the Clip has timed out.
        /// </summary>
        /// <returns>
        /// True if the clip timeout is exceeded, otherwise False.
        /// </returns>
        public bool getExpired()
        {
            if (timeout == -1) return false;
            if(Time.time > (time + timeout))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Scales the volume by master volume * sfx volume
        /// </summary>
        /// <returns></returns>
        public float getScaledVolume()
        {
            return volume * Settings.masterVolume * Settings.sfxVolume;
        }
    }

    public class AudioHandler : MonoBehaviour
    {
        public static AudioHandler instance;
        AudioSource musicTrack;
        AudioSource[] sfxTracks = new AudioSource[16];
        Queue<Clip> audioQueue = new Queue<Clip>();
        
        public void Awake()
        {
            instance = this;
            //Initialize
            musicTrack = gameObject.AddComponent<AudioSource>();
            musicTrack.loop = true;
            for(int i = 0; i < sfxTracks.Length; i++)
            {
                sfxTracks[i] = gameObject.AddComponent<AudioSource>();
            }
        }

        private void Update()
        {
            //Check if queue is empty
            if (!(audioQueue.Count > 0)) return;
            //Check if available track for sfx
            foreach(AudioSource sfxSource in sfxTracks)
            {
                if (!sfxSource.isPlaying)
                {
                    //Grab most recent clip and Deque it
                    Clip c = audioQueue.Dequeue();
                    if (c.clip == null)
                    {
                        if (audioQueue.Count > 0)
                        {
                            continue;
                        }
                        else
                        {
                            return;
                        }
                    }
                    //If clip is not expired, play it
                    if (!c.getExpired())
                    {
                        sfxSource.volume = c.getScaledVolume();
                        sfxSource.pitch = c.pitch + Random.Range(-c.variance, c.variance);
                        sfxSource.time = Mathf.Min(c.clip.length, c.startTime);
                        sfxSource.PlayOneShot(c.clip);
                        Debug.Log("played clip");
                    }
                    //Check if any more values in queue. If so, continue, otherwise, return
                    if(audioQueue.Count > 0)
                    {
                        continue;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// This ignores the Queue, but may not play if queue is actually full.
        /// </summary>
        /// <param name="clip">The Audio Clip to be played</param>
        /// <returns>True if clip is successfully played, otherwise returns False.</returns>
        public bool TryPlayClip(AudioClip clip)
        {
            foreach (AudioSource sfxSource in sfxTracks)
            {
                if (!sfxSource.isPlaying)
                {
                    sfxSource.PlayOneShot(clip);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Add a clip to the queue.
        /// </summary>
        /// <param name="clip">The Audioclip to be played</param>
        /// <param name="volume">The volume to play the clip at</param>
        /// <param name="pitch">The pitch to play the clip at</param>
        /// <param name="timeout">Won't play if it takes more than x seconds. -1 to disable</param>
        /// <param name="time">Time in seconds to start the AudioClip at</param>
        /// <param name="listener">Who should hear the audio clip? None for everyone</param>
        public void queueClip(AudioClip clip, float volume = 1, float pitch = 1, float timeout = -1, float time = 0)
        {
            audioQueue.Enqueue(new Clip(clip, volume, pitch, timeout, time));
        }
        /// <summary>
        /// Add a clip to the queue. This will play unless the clips timeout is exceeded.
        /// </summary>
        /// <param name="clip">The Clip containing the timeout and Audio Clip to be played</param>
        public void queueClip(Clip clip)
        {
            audioQueue.Enqueue(clip);
        }
    }
}
