using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplayer : MonoBehaviour
{
    public static Multiplayer multiplayer;
    public GameObject title;
    public GameObject Instructions;
    // Start is called before the first frame update
    void Start()
    {
        multiplayer = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
        public void multi1()
    {
        DeckOfCards.deckOfCards.multi = 1;
        title.SetActive(false);
        RebuildHUD();
    }

    public void multi2()
    {
        DeckOfCards.deckOfCards.multi = 2;
        title.SetActive(false);
        RebuildHUD();
    }

    public void multi3()
    {
        DeckOfCards.deckOfCards.multi = 3;
        title.SetActive(false);
        RebuildHUD();
    }

    public void multi4()
    {
        DeckOfCards.deckOfCards.multi = 4;
        title.SetActive(false);
        RebuildHUD();
    }

      public void ShowInstructions()
    {
        Instructions.SetActive(true);
    }

    public void HideInstructions()
    {
        Instructions.SetActive(false);
    }

    public void RebuildHUD()
    {
        DeckOfCards.deckOfCards.BuildGame();
    }
}
