using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharCard : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    //MANAGE DRAG & FLIP
    RectTransform rect;
    public bool draggable = true;
    public bool dragging;
    public bool flipping;
    Vector3 drag_start;
    Vector3 card_onDragOffset = new Vector3(0, 10.0f, 0);
    Vector3 card_origin = new Vector3(0, 0, 0);
    public float card_minGapToValid = 200f;
    public float quat_flipZlimit = 179.9f;
    public Quaternion quat_flip;

    //MANAGE EYES
    public RectTransform rect_eyes;
    Vector3 eye_origin;
    Vector3 eye_target;
    float eye_maxgap = 10f;

    //MANAGE ANSWER
    public Image img_bgAnswerText;
    public Text txt_bgAnswerText;
    float fadeTarget = 0f;
    float fadeMax = 0.8f;

    //MANAGE CARD
    public Card activeCard;
    public Color bg_targetColor;
    public Image img_charbackground;
    public Image img_character;
    public AudioSource as_dialog;
    public Text txt_CardCharTitle;
    public Text txt_CardDialog;


    public void Start()
    {
        //print("RectTransform: " + );
        card_origin = rect.localPosition;
        eye_origin = rect_eyes.localPosition;
        InvokeRepeating("newEyePosition", 0, 5f);
    }
    public void Awake()
    {
        rect = GetComponent<RectTransform>();
        quat_flip = Quaternion.Euler(0, quat_flipZlimit, 0);
    }

    public void newEyePosition()
    {
        eye_target = eye_origin + new Vector3(Random.Range(-eye_maxgap, eye_maxgap), Random.Range(-eye_maxgap, eye_maxgap) / 2, 0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MNG_Game.instance.txt_informations.text = "Glissez la carte sur un côté pour répondre à la situation";
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
        if (!draggable) return;
        Vector3 diff = Input.mousePosition - drag_start;
        if (diff.x > card_minGapToValid)
        {
            FlipCard();
            Invoke("apply_rightEffect",0.5f);
        }
        else if (diff.x < -card_minGapToValid)
        {
            FlipCard();
            Invoke("apply_leftEffect", 0.5f);
        }
        else
        {
            print(">> NOSWIPE DETECTED!");
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!draggable) return;
        dragging = true;
        drag_start = Input.mousePosition;
        MNG_Game.instance.src_sfx.PlayOneShot(MNG_Game.instance.aclp_paper);

    }
    public void Update()
    {
        if (dragging)
        {
            Vector3 diff = Input.mousePosition - drag_start;
            diff.y *= 0.1f;
            rect.localPosition = card_origin
                + card_onDragOffset
                + diff * 0.3f;
            rect.localRotation = Quaternion.Euler(0, 0, (diff.x > 0 ? -1 : 1) * diff.magnitude * 0.03f);

            //right swiping
            if (diff.x > card_minGapToValid)
            {
                fadeTarget = fadeMax;
                txt_bgAnswerText.text = activeCard.AnswerRight_string;
                for (int i = 0; i < MNG_Game.instance.categoryList.Count; i++)
                    MNG_Game.instance.categoryList[i].setImpact(activeCard.right_categoryEffect[i].impact);
            }
            //left swiping
            else if (diff.x < -card_minGapToValid)
            {
                fadeTarget = fadeMax;
                txt_bgAnswerText.text = activeCard.AnswerLeft_string;
                for (int i = 0; i < MNG_Game.instance.categoryList.Count; i++)
                    MNG_Game.instance.categoryList[i].setImpact(activeCard.left_categoryEffect[i].impact);
            }
            //no swiping
            else
            {
                fadeTarget = 0;
                txt_bgAnswerText.text = "";
                for (int i = 0; i < MNG_Game.instance.categoryList.Count; i++)
                    MNG_Game.instance.categoryList[i].setImpact(0);
            }
        }
        else if (flipping)
        {
            rect.localRotation = Quaternion.Lerp(rect.localRotation, quat_flip, 0.1f);
            rect.localPosition = Vector3.Lerp(rect.localPosition, card_origin, 0.1f);
            if (rect.localRotation.eulerAngles.y > quat_flipZlimit - 0.5f)
            {
                //print("TODO FLIPSTATE");
                //flipping = false;
            }
        }
        else
        {
            rect.localPosition = Vector3.Lerp(rect.localPosition, card_origin, 0.1f);
            rect.localRotation = Quaternion.Lerp(rect.localRotation, Quaternion.identity, 0.1f);
        }
        rect_eyes.localPosition = Vector3.Lerp(rect_eyes.localPosition, eye_target, 0.08f);
        img_bgAnswerText.color = new Color(0, 0, 0, Mathf.Lerp(img_bgAnswerText.color.a, fadeTarget, 0.1f));
        img_charbackground.color = Color.Lerp(img_charbackground.color,bg_targetColor,0.1f);
    }

    //MANAGE CARD
    public void installCard(Card card)
    {
        print(">> INSTALLING CARD : " + card.name);
        //set activeCard = card;
        activeCard = card;

        //set img
        img_character.sprite = activeCard.character.sprite;
        //set targetcolor
        bg_targetColor = activeCard.character.backgroundColor;
        //set display text
        txt_CardCharTitle.text = activeCard.character.Char_Name;
        txt_CardDialog.text = activeCard.dialog_string;
        rect_eyes.gameObject.SetActive(activeCard.character.hasEyes);
        newEyePosition();
        //set txt_informations = "nouveau tour"
        MNG_Game.instance.txt_informations.text = "Nouveau tour !";
        MNG_Game.instance.src_sfx.PlayOneShot(activeCard.character.DialogClip);

        //unflip card
        UnflipCard();
    }
    public void ResetInstantCharCardDisplay()
    {
        print(">> ResetInstantCharCardDisplay ");
        //set img
        img_character.sprite = null;
        //set targetcolor
        bg_targetColor = new Color(255, 255, 255, 1);
        //set display text
        txt_CardCharTitle.text = "";
        txt_CardDialog.text = "";
        img_bgAnswerText.color = new Color(0, 0, 0, 0);
        fadeTarget = 0;
    }
    public void ResetCharCard()
    {
        print(">> ResetCharCard ");
        draggable = false;
        dragging = false;
        flipping = true;
        rect_eyes.gameObject.SetActive(false);
        //transform.localRotation = quat_flip;
        foreach (var cat in MNG_Game.instance.categoryList) cat.setImpact(0);

        txt_bgAnswerText.text = "";
        activeCard = null;
        ResetInstantCharCardDisplay();
    }

    //APPLYING EFFECT
    public void apply_leftEffect()
    {
        //set effect
        for (int i = 0; i < MNG_Game.instance.categoryList.Count; i++)
            if (MNG_Game.instance.categoryList[i].addValue(activeCard.left_categoryEffect[i].value))
                return;//Si on atteint un gameover on ne continue pas

        //addCard random in deck
        foreach (var card in activeCard.left_cardListToAdd)
            MNG_Game.instance.AddCardToRandomPosition(card);
        print("CONFIRM : apply_leftEffect");

        ResetCharCard();
        MNG_Game.instance.EndTurn();
    }
    public void apply_rightEffect()
    {
        //set effect
        for (int i = 0; i < MNG_Game.instance.categoryList.Count; i++)
            if (MNG_Game.instance.categoryList[i].addValue(activeCard.right_categoryEffect[i].value))
                return;//Si on atteint un gameover on ne continue pas

        //addCard random in deck
        foreach (var card in activeCard.right_cardListToAdd)
            MNG_Game.instance.AddCardToRandomPosition(card);
        print("CONFIRM : apply_rightEffect");

        ResetCharCard();
        MNG_Game.instance.EndTurn();
    }

    //FLIPPING
    public void FlipCard()
    {
        draggable = false;
        dragging = false;
        flipping = true;
        txt_bgAnswerText.text = "";
    }
    public void UnflipCard()
    {
        print(">> UnflipCard");
        draggable = true;
        dragging = false;
        flipping = false;
        txt_bgAnswerText.text = "";
    }

}

