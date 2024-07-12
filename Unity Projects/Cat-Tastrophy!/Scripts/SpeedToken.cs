using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpeedToken : MonoBehaviour
{  
    public static SpeedToken speedToken;
    SphereCollider sc;   
    public AudioSource sfx;
    public AudioClip select;

    // Start is called before the first frame update
    void Start()
    {
        speedToken = this;
        sc = transform.GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    public void Update()
    {
        transform.Rotate(0, 75*Time.deltaTime, 0);
    }
    
    public void RestartButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        sfx.PlayOneShot(select);
    }
    public void ExitGame()
    {
        sfx.PlayOneShot(select);
        Application.Quit();
    }
}
