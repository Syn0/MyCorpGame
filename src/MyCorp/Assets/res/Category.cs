using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Category : MonoBehaviour, IPointerEnterHandler
{
    public string categoryname = "Template";
    public string categorydesc = "Template Desc";
    public GameObject go_smallPoint;
    public GameObject go_bigPoint;
    public Image img_progressbar;
    public float fillvalue = 0.5f; // 0..1
    public float cat_min = 0.0f;
    public float cat_max = 100.0f;
    public float cat_value = 0.5f;
    public Color targetcolor;
    public Color topColor = new Color(120, 250, 100, 1);
    public Color midColor = new Color(90, 230, 240, 1);
    public Color botColor = new Color(255, 60, 40, 1);
    public Card LooseCard;

    //TODO
    void Start()
    {
    }

    //Do this when the cursor enters the rect area of this selectable UI object.
    public void OnPointerEnter(PointerEventData eventData)
    {
        MNG_Game.instance.txt_informations.text = categorydesc;
    }
    //TODO
    void Update()
    {
        //Update fill
        img_progressbar.fillAmount = Mathf.Lerp(img_progressbar.fillAmount, cat_value / cat_max, 0.05f);
        img_progressbar.color = Color.Lerp(img_progressbar.color, targetcolor, 0.2f);
    }
    //TODO
    public void setImpact(int value)
    {
        go_smallPoint.SetActive(false);
        go_bigPoint.SetActive(false);
        if (value == 2) { go_bigPoint.SetActive(true); }
        if (value == 1) { go_smallPoint.SetActive(true); }
    }
    //TODO
    public bool addValue(float value)
    {
        cat_value += value;
        if (cat_value >= cat_max) cat_value = cat_max;
        else if (cat_value <= cat_min)
        {
            cat_value = cat_min;
            onMinLimit();
            return true;
        }
        //
        if (cat_value >= 0.75f * cat_max) targetcolor = topColor;
        else if (cat_value <= 0.25f * cat_max) targetcolor = botColor;
        else targetcolor = midColor;

        return false;
    }
    //TODO
    public void setValue(float value)
    {
        cat_value = value;
        if (cat_value >= cat_max) cat_value = cat_max;
        else if (cat_value <= cat_min) cat_value = cat_min;
        //
        if (cat_value >= 0.75f * cat_max) targetcolor = topColor;
        else if (cat_value <= 0.25f * cat_max) targetcolor = botColor;
        else targetcolor = midColor;
    }
    //TODO
    public void onMinLimit()
    {
        print(categoryname + " limite MIN atteinte");
        MNG_Game.instance.OnLoose(this);
    }

}
