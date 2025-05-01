using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class UpdateVideoAudio : MonoBehaviour
{
    public VideoPlayer player;
    public string nextScene = "Zone1";
    float waitOnStart = 1;

    // Update is called once per frame
    void Update()
    {
        for(ushort i = 0; i < player.audioTrackCount; i++)
        {
            player.SetDirectAudioVolume(i, Settings.masterVolume * Settings.sfxVolume);
        }
        if(!player.isPlaying && waitOnStart <= 0)
        {
            SceneManager.LoadScene(nextScene);
        }
        waitOnStart -= Time.deltaTime;
    }
}
