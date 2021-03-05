using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] bgm;
    public AudioSource[] sfx;

    public static AudioManager instance;

    public bool isBattle,isBattleEndTransition,isGameOver;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (FindObjectOfType<CameraController>())
        {
            if (!isBattle && !isBattleEndTransition && !isGameOver)
            {
                PlayBGM(FindObjectOfType<CameraController>().musicToPlay);
            }
            if (isBattle)
            {
                PlayBGM(FindObjectOfType<CameraController>().battleTrack);
            }
        }
       
    }

    public void PlaySFX(int trackNum)
    {
        sfx[trackNum].Play();
    }

    public void PlayBGM(int trackNum)
    {
        if (!bgm[trackNum].isPlaying)
        {
            StopMusic();

            if(trackNum < bgm.Length)
            {
                bgm[trackNum].Play();
            }
        }
    }

    public void StopMusic()
    {
        for(int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
}
