using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlay : MonoBehaviour
{
    public VideoClip v1,v2;
    public GameObject child;
    public GameObject topSubtitle;
    public GameObject midSubtitle;
    public GameObject bottomSubtitle;
    private bool active;
    private float playTime;
    private int videoIndex;
    private GameManager gameManager;
    private UIManager uiManager;
    private VideoPlayer vp;
    private int frame;
    // Start is called before the first frame update
    void Start()
    {
        active = false;
        playTime = 0f;
        videoIndex = -1;
        frame = 0;
        gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;
        uiManager.registerVideoPlay(this);
        vp = child.GetComponent<VideoPlayer>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            playTime = playTime + Time.unscaledDeltaTime;
            switch(videoIndex)
            {
                case 0:
                    if (playTime > 2f)
                    {
                        endPlay();
                    }
                    else {
                    }
                    break;
            }
        }
    }

    public void playVideo(int index)
    {
        switch (index)
        {
            case 0:
                vp.clip = v1;
                break;
        }
        frame = 0;
        active = true;
        videoIndex = index;
        playTime = 0f;
        vp.isLooping = false;
        gameObject.SetActive(true);
    }

    public void endPlay() {
        switch (videoIndex)
        {
            case 0:
                uiManager.showMask();
                gameManager.realStartNewGame();
                break;
        }
        active = false;
        videoIndex = -1;
        uiManager.exitUI();
        gameObject.SetActive(false);
    }
}
