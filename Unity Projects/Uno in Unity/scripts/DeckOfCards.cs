using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeckOfCards : MonoBehaviour
{
    public static DeckOfCards deckOfCards;
    System.Random rnd = new System.Random();
    int rndNum;
    public Card testCard;
    public Sprite img;
    public Card[] deck;
    public int index;
    public Player[] players;
    public Card[] gameBoard;
    public int whichPlayeramI = 0;
    public GameObject cardImagePrefab;
    public GameObject layoutMasterEmpty;
    public GameObject left;
    public GameObject right;
    public string cardInfoText = "";
    public int currentPlayerIndex = 0;
    public int currentCardIndex = -1;
    public bool canEnd;
    public bool hasChosen;
    public Transform canvas;
    public Transform middlePileEmpty;
    public Transform ColorPicker;
    public bool selectColor = false;
    public Card chosenCard;
    public Sprite[] cardImages;
    public Transform DeckDraw;
    public float thinkTimer;
    public float timeDelay;
    public Transform P2Layout;
    public Transform P3Layout;
    public Transform P4Layout;
    public bool endgameTick = false;
    public bool cardEffectActivated = true;
    public bool cardChecker = true;
    public GameObject leftScroll;
    public GameObject rightScroll;
    public GameObject P1Outline;
    public GameObject P2Outline;
    public GameObject P3Outline;
    public GameObject P4Outline;
    public GameObject EndScreen;
    public GameObject panel;
    public int multi = 1;
    public bool startgame = false;
    public AudioClip draw;
    public AudioClip colorpick;
    public AudioClip skipsound;
    public AudioClip drawtwosound;
    public AudioClip badgameover;
    public AudioClip draw4sound;
    public AudioClip reversesound;
    public AudioClip playcard;
    public AudioClip wingame;
    public AudioClip opponentplay;
    public AudioClip opponentdraw;
    public AudioClip invalidplay;
    public AudioClip endgamescreen;
    public AudioClip unobell;
    public AudioSource[] allAudioSources;

    public int scroll = 0;

    public bool reverse = false;
    public bool skip = false;

    private void Awake()
    {
        deckOfCards = this;
    }

    void Start()
    {
        deck = new Card[108];
        players = new Player[4];
        gameBoard = new Card[0];
        Card[] tempHand = new Card[0];
        for (int i = 0; i < players.Length; i++)
        {
            Player p = new Player();
            p.hand = tempHand;
            p.index = i;
            p.username = "Player " + (i + 1);
            players[i] = p;
            if (i == 0)
            {
                players[i].layout = layoutMasterEmpty.transform;
            }
            if (i == 1)
            {
                players[i].layout = P2Layout.transform;
            }
            if (i == 2)
            {
                players[i].layout = P3Layout.transform;
            }
            if (i == 3)
            {
                players[i].layout = P4Layout.transform;
            }
        }

        /* back, 1-9 dt rev skp blue red green yellow,
        0 brgy, wild brgy, wild 4 brgy*/

        for (int j = 0; j <= 1; j++)
        {
            // need a variable to assign and match up with imageIndex for cards
            int imgIterater = 1;
            for (int i = 1; i <= 12; i++)
            {
                if (index == 0)
                {
                    Card temp0 = new Card();
                    temp0.value = 0;
                    temp0.suit = Card.Suit.blue;
                    temp0.imageIndex = 49;
                    deck[index] = temp0;
                    index++;

                }
                if (index == 1)
                {
                    Card temp0 = new Card();
                    temp0.value = 0;
                    temp0.suit = Card.Suit.red;
                    temp0.imageIndex = 50;
                    deck[index] = temp0;
                    index++;
                }
                if (index == 2)
                {
                    Card temp0 = new Card();
                    temp0.value = 0;
                    temp0.suit = Card.Suit.green;
                    temp0.imageIndex = 51;
                    deck[index] = temp0;
                    index++;
                }
                if (index == 3)
                {
                    Card temp0 = new Card();
                    temp0.value = 0;
                    temp0.suit = Card.Suit.yellow;
                    temp0.imageIndex = 52;
                    deck[index] = temp0;
                    index++;
                }

                Card temp = new Card();
                temp.value = i;
                temp.suit = Card.Suit.blue;
                temp.imageIndex = imgIterater;
                deck[index] = temp;
                index++;
                imgIterater++;

            }
            for (int i = 1; i <= 12; i++)
            {
                Card temp = new Card();
                temp.value = i;
                temp.suit = Card.Suit.red;
                temp.imageIndex = imgIterater;
                deck[index] = temp;
                index++;
                imgIterater++;
            }
            for (int i = 1; i <= 12; i++)
            {
                Card temp = new Card();
                temp.value = i;
                temp.suit = Card.Suit.green;
                temp.imageIndex = imgIterater;
                deck[index] = temp;
                index++;
                imgIterater++;
            }
            for (int i = 1; i <= 12; i++)
            {
                Card temp = new Card();
                temp.value = i;
                temp.suit = Card.Suit.yellow;
                temp.imageIndex = imgIterater;
                deck[index] = temp;
                index++;
                imgIterater++;
            }
            for (int i = 0; i <= 1; i++)
            {
                Card temp = new Card();
                temp.value = 13;
                temp.suit = Card.Suit.black;
                temp.imageIndex = 53;
                deck[index] = temp;
                index++;
            }
            for (int i = 0; i <= 1; i++)
            {
                Card temp = new Card();
                temp.value = 14;
                temp.suit = Card.Suit.black;
                temp.imageIndex = 58;
                deck[index] = temp;
                index++;
            }
        }
        P1Outline.SetActive(true);
        Shuffle();
        // deal cards to each player
        for (int i = 0; i < players.Length; i++)
        {
            // deal exactly 5 cards to each player to start
            for (int j = 0; j < 5; j++)
            {
                DealCards(players[i]);
            }
            if (i != 0)
            {
                newOpponentHand(players[i]);
            }
        }
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].index == 0)
            {
                whichPlayeramI = i;
            }
        }
        for (int i = 0; i < players[whichPlayeramI].hand.Length; i++)
        {
            ModifiedShowCard(players[whichPlayeramI].hand[i], players[whichPlayeramI]);
        }
        newHand();
    }

    public void Shuffle()
    {
        int replacements = UnityEngine.Random.Range(30, 109);
        for (int i = 0; i < replacements; i++)
        {
            int a = UnityEngine.Random.Range(0, 108);
            int b = UnityEngine.Random.Range(0, 108);

            Card one = deck[a];
            Card two = deck[b];
            Card three = deck[a];

            one = two;
            two = three;

            deck[a] = one;
            deck[b] = two;
        }
    }
    public void DealCards(Player p)
    {
        // resize hand
        Card[] afterDraw = new Card[p.hand.Length + 1];
        p.hand.CopyTo(afterDraw, 0);
        afterDraw[p.hand.Length] = deck[0];
        p.hand = afterDraw;

        //remove card from deck
        Card[] tempDeck = new Card[deck.Length - 1];
        for (int i = 1; i < deck.Length; i++)
        {
            tempDeck[i - 1] = deck[i];
        }
        deck = tempDeck;
        AudioSource audio = GetComponent<AudioSource>();
        if ((currentPlayerIndex <= multi - 1 && multi != 2) || (multi == 2 && currentPlayerIndex != 1 && currentPlayerIndex != 3))
        {
            audio.PlayOneShot(draw);
        }
    }
    public void PlayCard(Player p, int selectedCard)
    {
        Card selection = p.hand[selectedCard];
        if ((currentPlayerIndex <= multi - 1 && multi != 2) || (multi == 2 && currentPlayerIndex != 1 && currentPlayerIndex != 3))
        {
            if (selection.value != gameBoard[0].value && selection.suit != gameBoard[0].suit && selection.suit != Card.Suit.black)
            {
                cardChecker = false;
                return;
            }
        }
        if (gameBoard.Length > 1)
        {
            cardEffectActivated = false;
        }
        cardChecker = true;
        Card[] tempGameBoard = new Card[gameBoard.Length + 1];
        gameBoard.CopyTo(tempGameBoard, 1);
        tempGameBoard[0] = selection;
        gameBoard = tempGameBoard;
        Discard(selection);
        if (selection.suit == Card.Suit.black)
        {
            ChooseColor();
        }
        // remove selected card from player hand
        Card[] tempHand = new Card[p.hand.Length - 1];
        for (int i = 0; i < p.hand.Length; i++)
        {
            if (i < selectedCard)
            {
                tempHand[i] = p.hand[i];
            }
            if (i > selectedCard)
            {
                tempHand[i - 1] = p.hand[i];
            }
        }
        p.hand = tempHand;
        newHand();
    }
    // discard works like play except without affecting hand lengths
    public void Discard(Card c)
    {
        GameObject newestPrefab = Instantiate(cardImagePrefab);
        newestPrefab.transform.SetParent(middlePileEmpty.transform);

        switch (currentPlayerIndex)
        {
            case 0:
                newestPrefab.GetComponent<RectTransform>().localPosition = Vector3.zero + Vector3.up * -10;
                break;
            case 1:
                newestPrefab.GetComponent<RectTransform>().localPosition = Vector3.zero + Vector3.right * 10;
                break;
            case 2:
                newestPrefab.GetComponent<RectTransform>().localPosition = Vector3.zero + Vector3.up * 10;
                break;
            case 3:
                newestPrefab.GetComponent<RectTransform>().localPosition = Vector3.zero + Vector3.right * -10;
                break;
        }
        newestPrefab.GetComponent<Image>().sprite = cardImages[c.imageIndex];

        CardInfo ci = newestPrefab.GetComponent<CardInfo>();
        ci.card = c;

    }
    public void ModifiedShowCard(Card c, Player p)
    {
        if (multi == 2 && p.index == 2)
        {
            GameObject newestPrefab = Instantiate(cardImagePrefab);
            newestPrefab.transform.SetParent(p.layout.transform);
            newestPrefab.GetComponent<Image>().sprite = cardImages[c.imageIndex];
            CardInfo ci = newestPrefab.GetComponent<CardInfo>();
            ci.card = c;
            return;
        }
        if (p.index == 0)
        {
            GameObject newestPrefab = Instantiate(cardImagePrefab);
            newestPrefab.transform.SetParent(layoutMasterEmpty.transform);
            newestPrefab.GetComponent<Image>().sprite = cardImages[c.imageIndex];
            CardInfo ci = newestPrefab.GetComponent<CardInfo>();
            ci.card = c;
        }
        else if (p.index == 2 && multi >= 3)
        {
            GameObject newestPrefab = Instantiate(cardImagePrefab);
            newestPrefab.transform.SetParent(p.layout.transform);
            newestPrefab.GetComponent<Image>().sprite = cardImages[c.imageIndex];
            CardInfo ci = newestPrefab.GetComponent<CardInfo>();
            ci.card = c;
        }
        else if (p.index == 1 && multi >= 2)
        {
            if (multi == 2)
            {
                return;
            }
            GameObject newestPrefab = Instantiate(cardImagePrefab);
            newestPrefab.transform.SetParent(p.layout.transform);
            newestPrefab.GetComponent<Image>().sprite = cardImages[c.imageIndex];
            CardInfo ci = newestPrefab.GetComponent<CardInfo>();
            ci.card = c;
            newestPrefab.transform.Rotate(0, 0, 90, Space.Self);
        }
        else if (p.index == 3 && multi >= 4)
        {
            GameObject newestPrefab = Instantiate(cardImagePrefab);
            newestPrefab.transform.SetParent(p.layout.transform);
            newestPrefab.GetComponent<Image>().sprite = cardImages[c.imageIndex];
            CardInfo ci = newestPrefab.GetComponent<CardInfo>();
            ci.card = c;
            newestPrefab.transform.Rotate(0, 0, -90, Space.Self);
        }
    }

    public void ChooseCard(Card c)
    {
        chosenCard = c;
        for (int i = 0; i < players[currentPlayerIndex].hand.Length; i++)
        {
            if (c == players[currentPlayerIndex].hand[i])
            {
                currentCardIndex = i;
            }
        }
        hasChosen = true;
    }

    public void EndTurn()
    {
        currentCardIndex = -1;
        thinkTimer = 0;
        timeDelay = 2;
        newOpponentHand(players[currentPlayerIndex]);
        if (players[currentPlayerIndex].hand.Length == 1)
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(unobell);
        }
        if (players[currentPlayerIndex].hand.Length == 0)
        {
            AudioSource audioend = GetComponent<AudioSource>();
            if ((currentPlayerIndex > multi - 1 && multi != 2) || (multi == 2 && currentPlayerIndex != 0 && currentPlayerIndex != 2))
            {
                audioend.PlayOneShot(badgameover);
            }
            else
            {
                audioend.PlayOneShot(wingame);
            }
            timeDelay = 3;
            if (!endgameTick)
            {
                endgameTick = true;
            }
            return;
        }
        if (gameBoard[0].value == 11 && !cardEffectActivated)
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(reversesound);
            reverse = !reverse;
            cardEffectActivated = true;
        }
        if (gameBoard[0].value == 12 && !cardEffectActivated)
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(skipsound);
            skip = true;
            cardEffectActivated = true;
        }
        if (!reverse)
        {
            if (skip && currentPlayerIndex < players.Length - 1)
            {
                currentPlayerIndex++;
                skip = false;
            }
            else if (skip && currentPlayerIndex == players.Length - 1)
            {
                currentPlayerIndex = 0;
                skip = false;
            }
            if (currentPlayerIndex == players.Length - 1)
            {
                currentPlayerIndex = 0;
            }
            else
            {
                currentPlayerIndex++;
            }
        }
        else
        {
            if (skip && currentPlayerIndex == 0)
            {
                currentPlayerIndex = players.Length - 1;
                skip = false;
            }
            else if (skip && currentPlayerIndex > 0)
            {
                currentPlayerIndex--;
                skip = false;
            }
            if (currentPlayerIndex == 0)
            {
                currentPlayerIndex = players.Length - 1;
            }
            else
            {
                currentPlayerIndex--;
            }
        }
        if (gameBoard[0].value == 10 && !cardEffectActivated)
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(drawtwosound);
            DealCards(players[currentPlayerIndex]);
            DealCards(players[currentPlayerIndex]);
            cardEffectActivated = true;
            newOpponentHand(players[currentPlayerIndex]);
            // if user draws, show their new hand
            newHand();
        }
        if (gameBoard[0].value == 14 && !cardEffectActivated)
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(draw4sound);
            DealCards(players[currentPlayerIndex]);
            DealCards(players[currentPlayerIndex]);
            DealCards(players[currentPlayerIndex]);
            DealCards(players[currentPlayerIndex]);
            cardEffectActivated = true;
            newOpponentHand(players[currentPlayerIndex]);
            newHand();
        }
        P1Outline.SetActive(false);
        P2Outline.SetActive(false);
        P3Outline.SetActive(false);
        P4Outline.SetActive(false);
        switch (currentPlayerIndex)
        {
            case 0:
                P1Outline.SetActive(true);
                break;
            case 1:
                P2Outline.SetActive(true);
                break;
            case 2:
                P3Outline.SetActive(true);
                break;
            case 3:
                P4Outline.SetActive(true);
                break;
        }
        newHand();
    }

    // Update is called once per frame
    void Update()
    {
        if (startgame)
        {
            timeDelay = 1;
            thinkTimer += Time.deltaTime;
            if (thinkTimer > timeDelay)
            {
                startgame = false;
                newHand();
                for (int i = 1; i < 4; i++)
                {
                    newOpponentHand(players[i]);
                }

                for (int i = 0; i < 4; i++)
                {
                    GameObject DeckPrefab = Instantiate(cardImagePrefab);
                    DeckPrefab.transform.SetParent(DeckDraw.transform);
                    DeckPrefab.GetComponent<Image>().sprite = cardImages[0];
                    DeckPrefab.GetComponent<RectTransform>().localPosition = Vector3.zero + Vector3.up * (i * 3);
                    CardInfo di = DeckPrefab.GetComponent<CardInfo>();
                    di.card = new Card();
                    di.card.value = 16;
                }
                 Card[] tempGameBoard = new Card[gameBoard.Length + 1];
                gameBoard.CopyTo(tempGameBoard, 1);
                tempGameBoard[0] = deck[0];
                gameBoard = tempGameBoard;
                // remove top card from deck and play it
                Card[] tempDeck = new Card[deck.Length - 1];
                for (int i = 1; i < deck.Length; i++)
                {
                    tempDeck[i - 1] = deck[i];
                }
                deck = tempDeck;
                Discard(gameBoard[0]);
                panel.SetActive(true);
                if (gameBoard[0].suit == Card.Suit.black)
                {
                    ChooseColor();
                }
            }
        }
        if (multi == 2 && currentPlayerIndex != 0 && currentPlayerIndex != 2)
        {
            thinkTimer += Time.deltaTime;
            if (thinkTimer > timeDelay)
            {
                if (endgameTick)
                {
                    endgameTick = false;
                    EndGame(players[currentPlayerIndex]);
                    currentPlayerIndex = 0;
                    return;
                }
                hasChosen = true;
                ComputerPlayYourChosenCard();
                return;
            }
        }
        if (currentPlayerIndex > multi - 1 && multi != 2)
        {
            thinkTimer += Time.deltaTime;
            if (thinkTimer > timeDelay)
            {
                if (endgameTick)
                {
                    endgameTick = false;
                    EndGame(players[currentPlayerIndex]);
                    currentPlayerIndex = 0;
                    return;
                }
                hasChosen = true;
                ComputerPlayYourChosenCard();
            }
        }
        if (currentPlayerIndex <= multi - 1 && endgameTick)
        {
            thinkTimer += Time.deltaTime;
            if (thinkTimer > timeDelay)
            {
                endgameTick = false;
                EndGame(players[currentPlayerIndex]);
            }
        }
        if (multi == 2 && endgameTick && currentPlayerIndex == 2)
        {
            thinkTimer += Time.deltaTime;
            if (thinkTimer > timeDelay)
            {
                endgameTick = false;
                EndGame(players[currentPlayerIndex]);
            }
        }
    }

    public void ChooseColor()
    {
        if ((currentPlayerIndex <= multi - 1 && multi != 2) || (multi == 2 && currentPlayerIndex != 1 && currentPlayerIndex != 3))
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(colorpick);
            if (gameBoard[0].value == 14)
            {
                selectColor = true;
                //blue
                GameObject PrefabBlue = Instantiate(cardImagePrefab);
                PrefabBlue.transform.SetParent(ColorPicker.transform);
                PrefabBlue.GetComponent<Image>().sprite = cardImages[59];
                CardInfo ciblue = PrefabBlue.GetComponent<CardInfo>();
                ciblue.card.suit = Card.Suit.blue;
                ciblue.card.imageIndex = 59;
                ciblue.card.value = 19;
                //red
                GameObject PrefabRed = Instantiate(cardImagePrefab);
                PrefabRed.transform.SetParent(ColorPicker.transform);
                PrefabRed.GetComponent<Image>().sprite = cardImages[60];
                CardInfo cired = PrefabRed.GetComponent<CardInfo>();
                cired.card.suit = Card.Suit.red;
                cired.card.imageIndex = 60;
                cired.card.value = 19;
                //green
                GameObject PrefabGreen = Instantiate(cardImagePrefab);
                PrefabGreen.transform.SetParent(ColorPicker.transform);
                PrefabGreen.GetComponent<Image>().sprite = cardImages[61];
                CardInfo cigreen = PrefabGreen.GetComponent<CardInfo>();
                cigreen.card.suit = Card.Suit.green;
                cigreen.card.imageIndex = 61;
                cigreen.card.value = 19;
                //yellow
                GameObject PrefabYellow = Instantiate(cardImagePrefab);
                PrefabYellow.transform.SetParent(ColorPicker.transform);
                PrefabYellow.GetComponent<Image>().sprite = cardImages[62];
                CardInfo ciyellow = PrefabYellow.GetComponent<CardInfo>();
                ciyellow.card.suit = Card.Suit.yellow;
                ciyellow.card.imageIndex = 62;
                ciyellow.card.value = 19;
            }
            else
            {
                selectColor = true;
                //blue
                GameObject PrefabBlue = Instantiate(cardImagePrefab);
                PrefabBlue.transform.SetParent(ColorPicker.transform);
                PrefabBlue.GetComponent<Image>().sprite = cardImages[54];
                CardInfo ciblue = PrefabBlue.GetComponent<CardInfo>();
                ciblue.card.suit = Card.Suit.blue;
                ciblue.card.imageIndex = 54;
                ciblue.card.value = 15;
                //red
                GameObject PrefabRed = Instantiate(cardImagePrefab);
                PrefabRed.transform.SetParent(ColorPicker.transform);
                PrefabRed.GetComponent<Image>().sprite = cardImages[55];
                CardInfo cired = PrefabRed.GetComponent<CardInfo>();
                cired.card.suit = Card.Suit.red;
                cired.card.imageIndex = 55;
                cired.card.value = 15;
                //green
                GameObject PrefabGreen = Instantiate(cardImagePrefab);
                PrefabGreen.transform.SetParent(ColorPicker.transform);
                PrefabGreen.GetComponent<Image>().sprite = cardImages[56];
                CardInfo cigreen = PrefabGreen.GetComponent<CardInfo>();
                cigreen.card.suit = Card.Suit.green;
                cigreen.card.imageIndex = 56;
                cigreen.card.value = 15;
                //yellow
                GameObject PrefabYellow = Instantiate(cardImagePrefab);
                PrefabYellow.transform.SetParent(ColorPicker.transform);
                PrefabYellow.GetComponent<Image>().sprite = cardImages[57];
                CardInfo ciyellow = PrefabYellow.GetComponent<CardInfo>();
                ciyellow.card.suit = Card.Suit.yellow;
                ciyellow.card.imageIndex = 57;
                ciyellow.card.value = 15;
            }
        }
        else
        {
            foreach (Card item in players[currentPlayerIndex].hand)
            {
                if (item.suit != Card.Suit.black)
                {
                    gameBoard[0].suit = item.suit;
                    if (gameBoard[0].value == 13)
                    {
                        if (gameBoard[0].suit == Card.Suit.blue)
                        {
                            gameBoard[0].imageIndex = 54;
                        }
                        else if (gameBoard[0].suit == Card.Suit.red)
                        {
                            gameBoard[0].imageIndex = 55;
                        }
                        else if (gameBoard[0].suit == Card.Suit.green)
                        {
                            gameBoard[0].imageIndex = 56;
                        }
                        else if (gameBoard[0].suit == Card.Suit.yellow)
                        {
                            gameBoard[0].imageIndex = 57;
                        }
                        else
                        {
                            rndNum = rnd.Next(54, 58);
                            gameBoard[0].imageIndex = rndNum;
                        }
                    }
                    else
                    {
                        if (gameBoard[0].suit == Card.Suit.blue)
                        {
                            gameBoard[0].imageIndex = 59;
                        }
                        else if (gameBoard[0].suit == Card.Suit.red)
                        {
                            gameBoard[0].imageIndex = 60;
                        }
                        else if (gameBoard[0].suit == Card.Suit.green)
                        {
                            gameBoard[0].imageIndex = 61;
                        }
                        else if (gameBoard[0].suit == Card.Suit.yellow)
                        {
                            gameBoard[0].imageIndex = 62;
                        }
                        else
                        {
                            rndNum = rnd.Next(59, 63);
                            gameBoard[0].imageIndex = rndNum;
                        }
                    }
                    if (gameBoard[0].suit == Card.Suit.black)
                    {
                        if (rndNum == 54 || rndNum == 59)
                        {
                            gameBoard[0].suit = Card.Suit.blue;
                        }
                        else if (rndNum == 55 || rndNum == 60)
                        {
                            gameBoard[0].suit = Card.Suit.red;
                        }
                        else if (rndNum == 56 || rndNum == 61)
                        {
                            gameBoard[0].suit = Card.Suit.green;
                        }
                        else if (rndNum == 57 || rndNum == 62)
                        {
                            gameBoard[0].suit = Card.Suit.yellow;
                        }
                    }
                    break;
                }
            }
            Discard(gameBoard[0]);
        }
    }
    public void newHand()
    {
        if (currentPlayerIndex > multi - 1 && multi != 2)
        {
            return;
        }
        if (multi == 2 && currentPlayerIndex != 0 && currentPlayerIndex != 2)
        {
            return;
        }
        foreach (Transform child in players[currentPlayerIndex].layout.transform)
        {
            Destroy(child.gameObject);
        }
        // if player hand is greater than 7, initiate scroll functionality
        if (currentPlayerIndex == 0 && players[0].hand.Length > 7)
        {
            if (scroll > 0)
            {
                leftScroll.SetActive(true);
            }
            if (scroll < players[0].hand.Length - 7)
            {
                rightScroll.SetActive(true);
            }
            if (scroll <= 0)
            {
                leftScroll.SetActive(false);
            }
            if (scroll >= players[0].hand.Length - 7)
            {
                rightScroll.SetActive(false);
            }
            for (int i = scroll; i < scroll + 7; i++)
            {
                try
                {
                    ModifiedShowCard(players[0].hand[i], players[0]);
                }
                catch
                {
                    break;
                }
            }
        }
        else
        {
            leftScroll.SetActive(false);
            rightScroll.SetActive(false);
            for (int i = 0; i < players[currentPlayerIndex].hand.Length; i++)
            {
                ModifiedShowCard(players[currentPlayerIndex].hand[i], players[currentPlayerIndex]);
            }
        }
    }

    public void newOpponentHand(Player p)
    {
        foreach (Transform child in p.layout)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < p.hand.Length; i++)
        {
            GameObject c = Instantiate(cardImagePrefab);
            c.transform.SetParent(p.layout);
            c.GetComponent<Image>().sprite = cardImages[0];
            if (p == players[0] && i == 6)
            {
                break;
            }
            if (p == players[1])
            {
                c.transform.Rotate(0, 0, 90, Space.Self);
            }
            if (p == players[2])
            {
                c.transform.Rotate(0, 0, 180, Space.Self);
            }
            if (p == players[3])
            {
                c.transform.Rotate(0, 0, -90, Space.Self);
            }
        }
    }
    public void PlayYourChosenCard()
    {
        if (hasChosen)
        {
            PlayCard(players[currentPlayerIndex], currentCardIndex);
            if (selectColor)
            {
                return;
            }
            if (!cardChecker)
            {
                AudioSource a = GetComponent<AudioSource>();
                a.clip = invalidplay;
                a.Play();
                return;
            }
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(playcard);
            EndTurn();
        }
        else
        {
            AudioSource a = GetComponent<AudioSource>();
            a.clip = invalidplay;
            a.Play();
        }
    }

    public void ComputerPlayYourChosenCard()
    {
        if (endgameTick)
        {
            EndGame(players[currentPlayerIndex]);
            return;
        }
        currentCardIndex = 0;
        foreach (Card item in players[currentPlayerIndex].hand)
        {
            if (gameBoard[0].suit == item.suit || gameBoard[0].value == item.value || item.suit == Card.Suit.black)
            {
                AudioSource audio = GetComponent<AudioSource>();
                audio.PlayOneShot(opponentplay);
                PlayCard(players[currentPlayerIndex], currentCardIndex);
                EndTurn();
                return;
            }
            currentCardIndex++;
        }
        DealCards(players[currentPlayerIndex]);
        AudioSource a = GetComponent<AudioSource>();
        a.PlayOneShot(opponentdraw);
        newOpponentHand(players[currentPlayerIndex]);

        EndTurn();
    }
    public void EndGame(Player p)
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource ad in allAudioSources)
        {
            ad.Stop();
        }
        // set delay so new music will play after short anthem
        AudioSource endaudio = GetComponent<AudioSource>();
        endaudio.clip = endgamescreen;
        endaudio.Play();
        TMPro.TextMeshProUGUI EndText = EndScreen.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        string name = "";
        switch (currentPlayerIndex)
        {
            case 0:
                name = "Blue";
                break;
            case 1:
                name = "Green";
                break;
            case 2:
                name = "Yellow";
                break;
            case 3:
                name = "Red";
                break;
        }
        EndText.text = "Game Over! The " + name + " player wins!";
        EndScreen.SetActive(true);
        currentPlayerIndex = 0;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("Game Screen");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void BuildGame()
    {        
        startgame = true;
    }
}

[Serializable]
public class Card
{
    //card values: 0-9 = 0-9, 10 = draw two, 11 = reverse, 12 = skip, 13 = wild, 
    // 14 = wild draw 4, 15 = color picker, 16 = card back (draw pile)
    public int value;
    public enum Suit { blue, red, yellow, green, black };
    public Suit suit;
    public int imageIndex;
}
[Serializable]
public class Player
{
    public Card[] hand;
    public int index;
    public string username;
    public Transform layout;
}
