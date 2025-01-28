using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tp2
{
    [System.Serializable]
    public class AudioSettings
    {
        public float sfxVolume = 1f;
        public float sfxPitch = 1f;
        public float startTime = 0f;
        public float timeout = -1f;
        public float cooldown = 0.1f;
        public AudioClip[] sfx = new AudioClip[2];
    }

    public class SfxHandler : MonoBehaviour
    {
        public AudioSettings audioSettings = new AudioSettings();
        float timer = 0;
        public void playClip(int i, bool overrideTimer = false)
        {
            if (getSfx(i) == null) return;
            playClip(getSfx(i), overrideTimer);
        }

        private void Update()
        {
            if(timer > 0)
            {
                timer -= Time.deltaTime;
                if(timer < 0)
                {
                    timer = 0;
                }
            }
        }

        public void playClip(bool b, bool overrideTimer = false)
        {
            if (b)
            {
                playClip(0, overrideTimer);
            }
            else
            {
                playClip(1, overrideTimer);
            }
        }

        public void playClip(AudioClip clip, bool overrideTimer = false)
        {
            if (timer > 0 && !overrideTimer) return;
            AudioHandler.instance.queueClip(clip, audioSettings.sfxVolume, audioSettings.sfxPitch, audioSettings.timeout, audioSettings.startTime);
            timer = audioSettings.cooldown;
        }

        public AudioClip getSfx(bool t)
        {
            if (t)
            {
                return audioSettings.sfx[0];
            }
            else
            {
                return audioSettings.sfx[1];
            }
        }
        
        public AudioClip getSfx(int i)
        {
            if(audioSettings.sfx.Length > i)
            {
                return audioSettings.sfx[i];
            }
            else
            {
                return null;
            }
        }
    }
}
