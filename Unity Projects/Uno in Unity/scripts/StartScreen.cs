using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game Screen");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
