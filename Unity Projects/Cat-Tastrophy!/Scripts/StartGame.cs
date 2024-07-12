using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public GameObject StartScreen;
    public static StartGame startGame;
    public GameObject Ai;
    public GameObject instructions;
    public GameObject pauseScreen;
    public AudioSource bgm;
    public AudioClip gameplayMusic;
    public AudioSource sfx;
    public AudioClip select;
    public GameObject controlScreen;
    // Start is called before the first frame update
    void Start()
    {
        startGame = this;
        Time.timeScale = 0;
    }
    void Update()
    {
        if (StartScreen.activeSelf == true)
        {
            Time.timeScale = 0;
        }
    }
    public void StartButton()
    {
        sfx.PlayOneShot(select);
        StartScreen.SetActive(false);
        Time.timeScale = 1;
        bgm.clip = gameplayMusic;
        bgm.Play();
    }
    public void Solo()
    {
        Ai.SetActive(false);
        StartButton();
    }
    public void OpenInfo()
    {
        sfx.PlayOneShot(select);
        if (pauseScreen.activeSelf == true)
        {
            return;
        }
        instructions.SetActive(true);
        Time.timeScale = 0;
    }
    public void CloseInfo()
    {
        sfx.PlayOneShot(select);
        instructions.SetActive(false);
        Time.timeScale = 1;
    }
    public void OpenControls()
    {
        sfx.PlayOneShot(select);
        controlScreen.SetActive(true);
    }
    public void CloseControls()
    {
        sfx.PlayOneShot(select);
        controlScreen.SetActive(false);
    }
}
