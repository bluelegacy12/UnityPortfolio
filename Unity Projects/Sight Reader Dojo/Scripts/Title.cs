using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public TextMeshProUGUI UpdateText;
    public GameObject credits;
    void Start()
    {
        UpdateText = GameObject.Find("Update Text").GetComponent<TextMeshProUGUI>();
    }
public void Soprano()
    {
        UpdateText.text = "Loading...";
        SceneManager.LoadScene("Sop", LoadSceneMode.Single);
    }
    public void Alto()
    {
        UpdateText.text = "Loading...";
        SceneManager.LoadScene("Alto", LoadSceneMode.Single);
    }
    public void Tenor()
    {
        UpdateText.text = "Loading...";
        SceneManager.LoadScene("Tenor", LoadSceneMode.Single);
    }
    public void Bass()
    {
        UpdateText.text = "Loading...";
        SceneManager.LoadScene("Bass", LoadSceneMode.Single);
    }
    public void CreditsButton()
    {
        credits.SetActive(true);
    }
    public void CloseCredits()
    {
        credits.SetActive(false);
    }
}
