using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void NextButton()
    {
        // Soprano.soprano.voiceRecord.Stop();
        Soprano.soprano.oSource.Stop();
        Soprano.soprano.timer = 0;
        Soprano.soprano.list.RemoveAt(Soprano.soprano.index);
        if (Soprano.soprano.list.Count == 0)
        {
            for (int i = 0; i < Soprano.soprano.Exercises.Length; i++)
            {
                Soprano.soprano.list.Add(i);
            }
        }
        Soprano.soprano.index = UnityEngine.Random.Range(0, Soprano.soprano.list.Count);
        Soprano.soprano.currentEx.sprite = Soprano.soprano.Exercises[Soprano.soprano.list.ElementAt(Soprano.soprano.index)]; 
        Soprano.soprano.AudioSource.clip = Soprano.soprano.Audios[Soprano.soprano.list.ElementAt(Soprano.soprano.index)];
        Soprano.soprano.AudioSource.Play();
        Soprano.soprano.activateRecord = false;
        Soprano.soprano.nextCover.SetActive(true);
        // Soprano.soprano.playVoiceCover.SetActive(true);
        Soprano.soprano.PlayOriginalCover.SetActive(true);
        Soprano.soprano.UpdateText.text = "You have 30 seconds to practice until the arpeggio plays again";
    }   
    public void DoneRecording()
    {
        // Microphone.End("");
        Soprano.soprano.doneCover.SetActive(true);
        Soprano.soprano.nextCover.SetActive(false);
        // Soprano.soprano.playVoiceCover.SetActive(false);
        Soprano.soprano.PlayOriginalCover.SetActive(false);
        Soprano.soprano.UpdateText.text = "Review the correct track and see how you did! Download the Android app or desktop version to use the voice record feature and play it back for direct comparison.";
        // Soprano.soprano.UpdateText.text = "Review your recording and compare it to the correct track. When you are ready to move on press 'Next'";
    }
    public void PlayOriginalRecording()
    {
        // Soprano.soprano.voiceRecord.Stop();
        Soprano.soprano.AudioSource.Stop();
        Soprano.soprano.oSource.clip = Soprano.soprano.Originals[Soprano.soprano.list.ElementAt(Soprano.soprano.index)];
        Soprano.soprano.oSource.Play();
    }
    /* public void PlayVoiceRecording()
    {
        Soprano.soprano.AudioSource.Stop();
        Soprano.soprano.oSource.Stop();
        Soprano.soprano.voiceRecord.Play();
    } */
    public void Menu()
    {
        SceneManager.LoadScene("Title Screen", LoadSceneMode.Single);
    }
}
