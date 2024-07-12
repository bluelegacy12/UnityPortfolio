using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Soprano : MonoBehaviour
{
    public static Soprano soprano;
    public GameObject Exercise;
    public AudioSource AudioSource;
    public AudioSource oSource;
    public TextMeshProUGUI UpdateText;
    public Sprite[] Exercises;
    public AudioClip[] Audios;
    public AudioClip[] Originals;
    public int index;
    public Button Next;
    public Button Done;
    // public Button PlayVoice;
    public Button PlayOriginal;
    public GameObject nextCover;
    public GameObject doneCover;
    // public GameObject playVoiceCover;
    public GameObject PlayOriginalCover;
    public Image currentEx;
    public List<int> list;
    public bool activateRecord = false;
    // public AudioSource voiceRecord;
    public int buttonCounter = 0;
    public float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        soprano = this;
        // voiceRecord = GameObject.Find("Voice Record").GetComponent<AudioSource>();
        oSource = GameObject.Find("Original Audio Source").GetComponent<AudioSource>();
        // create list of index options
        list = new List<int>();
        for (int i = 0; i < Exercises.Length; i++)
        {
            list.Add(i);
        }
        // randomly select index for exercise to be chosen
        index = UnityEngine.Random.Range(0, list.Count);
        currentEx = Exercise.GetComponent<Image>();
        currentEx.sprite = Exercises[index]; 
        AudioSource.clip = Audios[index];
        AudioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (activateRecord == false)
        {
            timer += Time.deltaTime;
            if (timer >= AudioSource.clip.length)
            {
                Record();
            }
        }
    }
    public void Record()
    {
        activateRecord = true;
        // voiceRecord.clip = Microphone.Start("", false, 150, 44100);
        // UpdateText.text = "Start! Recording Audio...";
        UpdateText.text = "Start! Recording feature only available on Android or Desktop version";
        doneCover.SetActive(false);
    }
}
