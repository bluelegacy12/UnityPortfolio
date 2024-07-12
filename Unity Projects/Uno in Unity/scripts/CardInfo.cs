using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Card card;
    public GameObject selectedObject;
    public bool hasChosen;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        selectedObject = this.gameObject;
        if (card.value != 20 && card.value != 16)
        {
            if (DeckOfCards.deckOfCards.currentPlayerIndex > DeckOfCards.deckOfCards.multi - 1 && DeckOfCards.deckOfCards.multi != 2)
            {
                return;
            }
            if (DeckOfCards.deckOfCards.multi == 2 && DeckOfCards.deckOfCards.currentPlayerIndex != 0 && DeckOfCards.deckOfCards.currentPlayerIndex != 2)
            {
                return;
            }
            switch (DeckOfCards.deckOfCards.currentPlayerIndex)
            {
                case 0:
                    transform.Translate(0, 15, 0);
                    break;
                case 1:
                    transform.Translate(0, 30, 0);
                    break;
                case 2:
                    transform.Translate(0, -15, 0);
                    break;
                case 3:
                    transform.Translate(0, 30, 0);
                    break;
            }
        }
        if (card.value == 16)
        {
            transform.Translate(0, 15, 0);
        }
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        selectedObject = null;
        if (card.value != 20 && card.value != 16)
        {
            if (DeckOfCards.deckOfCards.currentPlayerIndex > DeckOfCards.deckOfCards.multi - 1 && DeckOfCards.deckOfCards.multi != 2)
            {
                return;
            }
            if (DeckOfCards.deckOfCards.multi == 2 && DeckOfCards.deckOfCards.currentPlayerIndex != 0 && DeckOfCards.deckOfCards.currentPlayerIndex != 2)
            {
                return;
            }
            switch (DeckOfCards.deckOfCards.currentPlayerIndex)
            {
                case 0:
                    transform.Translate(0, -15, 0);
                    break;
                case 1:
                    transform.Translate(0, -30, 0);
                    break;
                case 2:
                    transform.Translate(0, 15, 0);
                    break;
                case 3:
                    transform.Translate(-0, -30, 0);
                    break;
            }
        }
        if (card.value == 16)
        {
            transform.Translate(0, -15, 0);
        }
    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (card.value == 20)
        {
            return;
        }
        if (DeckOfCards.deckOfCards.selectColor)
        {
            if (card.value == 15 || card.value == 19)
            {
                DeckOfCards.deckOfCards.selectColor = false;
                DeckOfCards.deckOfCards.Discard(card);
                DeckOfCards.deckOfCards.gameBoard[0].suit = card.suit;
                // 19 checks for user wild draw 4 with color change and we want the wild draw 4 effect to happen still
                if (card.value == 19)
                {
                    DeckOfCards.deckOfCards.gameBoard[0].value = 14;
                }
                foreach (Transform child in DeckOfCards.deckOfCards.ColorPicker.transform)
                {
                    Destroy(child.gameObject);
                }
                DeckOfCards.deckOfCards.EndTurn();
            }
            else
            {
                return;
            }
        }
        if (card.value == 17)
        {
            DeckOfCards.deckOfCards.scroll--;
            if (DeckOfCards.deckOfCards.scroll <= 0)
            {
                DeckOfCards.deckOfCards.scroll = 0;
            }
            DeckOfCards.deckOfCards.newHand();
            return;
        }
        if (card.value == 18)
        {
            DeckOfCards.deckOfCards.scroll++;
            if (DeckOfCards.deckOfCards.scroll >= DeckOfCards.deckOfCards.players[0].hand.Length - 7)
            {
                DeckOfCards.deckOfCards.scroll = DeckOfCards.deckOfCards.players[0].hand.Length - 7;
            }
            DeckOfCards.deckOfCards.newHand();
            return;
        }
        if (DeckOfCards.deckOfCards.currentPlayerIndex > DeckOfCards.deckOfCards.multi - 1 && DeckOfCards.deckOfCards.multi != 2)
        {
            return;
        }
        if (DeckOfCards.deckOfCards.multi == 2 && DeckOfCards.deckOfCards.currentPlayerIndex != 0 && DeckOfCards.deckOfCards.currentPlayerIndex != 2)
        {
            return;
        }
        if (card.value == 16)
        {
            DeckOfCards.deckOfCards.DealCards(DeckOfCards.deckOfCards.players[DeckOfCards.deckOfCards.currentPlayerIndex]);
            DeckOfCards.deckOfCards.newHand();
            DeckOfCards.deckOfCards.EndTurn();
            return;
        }
        
        DeckOfCards.deckOfCards.ChooseCard(card);
        DeckOfCards.deckOfCards.PlayYourChosenCard();
    }
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /* if (Input.GetKey(KeyCode.Alpha0))
        {
            DeckOfCards.deckOfCards.EndGame(DeckOfCards.deckOfCards.players[0]);
        } */
    }
}
