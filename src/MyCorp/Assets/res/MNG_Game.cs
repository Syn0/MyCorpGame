using System.Collections;
using System.Collections.Generic;
//using UnityEditor.AI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MNG_Game : MonoBehaviour
{
    //SINGLETON
    public static MNG_Game instance { get; private set; }
    private void Awake()
    {
        instance = this;
        //
        pnl_menu.SetActive(true);
        pnl_loose.SetActive(false);
        pnl_win.SetActive(false);
        //Cursor.SetCursor(tex_cursor, Vector2.zero, CursorMode.Auto);
        setWinPanel();

        //...
        print("MNG_Game : LOADED");
    }

    public AudioSource src_music;
    public AudioSource src_sfx;
    public AudioClip aclp_paper;
    public AudioClip aclp_panel;

    public Texture2D tex_cursor;
    public GameObject cnt_main;
    public GameObject pnl_loose;
    public GameObject pnl_win;
    public GameObject pnl_menu;
    public GameObject pnl_game;
    public GameObject pnl_credits;

    public CharCard charCard;

    public Text txt_informations;
    public Text txt_WinPanel;
    public Image img_WinPanel;
    public Text txt_LoosePanel;
    public Image img_LoosePanel;

    public List<Category> categoryList;
    public List<Card> introductionCardList;
    public List<Card> startCardList;
    public List<Card> cardDeck = new List<Card>();
    public Card winCard;
    //public GameObject pnl_help;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) sendCommand("btn_menu");

    }
    public void sendCommand(string cmd)
    {
        Debug.Log("[CMD] " + cmd);
        switch (cmd)
        {
            case "btn_credits":
                pnl_credits.SetActive(!pnl_credits.activeInHierarchy);
                break;
            case "btn_menu":
                pnl_menu.SetActive(!pnl_menu.activeInHierarchy);
                break;
            case "btn_quit":
                // save any game data here
                Quit();
                break;
            case "btn_play":
                StartNewGame();
                break;
            case "btn_restart":
                StartNewGame();
                break;
            default:
                print(">> [" + cmd + "] Commande inconnu");
                break;
        }
    }
    public void StartNewGame()
    {
        print("STARTING NEW GAME");
        pnl_menu.SetActive(false);
        pnl_loose.SetActive(false);
        pnl_win.SetActive(false);
        charCard.ResetCharCard();

        //set Deck = InitDeck
        cardDeck.Clear();
        cardDeck.AddRange(startCardList);
        //Shuffle Deck
        cardDeck.Shuffle();
        //Push IntroductionCard to start
        cardDeck.InsertRange(0, introductionCardList);
        //Set cat to middle or max
        foreach (Category cat in categoryList)
        {
            cat.setImpact(0);
            cat.setValue(50);
        }

        print("DECK IS READY");
        //Flip new first card
        newCard();
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
    }

    //█ GAMEPLAY ████████████████████████████████████████████████
    public void EndTurn()
    {
        if (!newCard()) OnWin();
    }
    public bool newCard()
    {
        print(">> PICK NEW CARD");
        if (cardDeck.Count == 0) { return false; }
        Card newCard = PickCard();
        print(">> NEW CARD name:" + newCard.name);
        charCard.installCard(newCard);
        return true;
    }
    public Card PickCard()
    {
        Card c = cardDeck[0];
        cardDeck.RemoveAt(0);
        return c;
    }
    public void AddCardToRandomPosition(Card card)
    {
        print("DECK AddCard:" + card.name);
        cardDeck.Insert(Random.Range(0, cardDeck.Count), card);
    }
    public void setWinPanel()
    {
        //set win text
        txt_WinPanel.text = winCard.dialog_string;
        //set win image
        img_WinPanel.sprite = winCard.character.sprite;
    }
    public void setLoosePanel(Category cat)
    {
        //set win text
        txt_LoosePanel.text = cat.LooseCard.dialog_string;
        //set win image
        img_LoosePanel.sprite = cat.LooseCard.character.sprite;
    }
    public void OnWin()
    {
        src_sfx.PlayOneShot(aclp_panel);
        pnl_win.SetActive(true);
        charCard.ResetCharCard();
    }
    public void OnLoose(Category cat)
    {
        src_sfx.PlayOneShot(aclp_panel);
        charCard.ResetCharCard();
        setLoosePanel(cat);
        pnl_loose.SetActive(true);
    }
}

public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}
