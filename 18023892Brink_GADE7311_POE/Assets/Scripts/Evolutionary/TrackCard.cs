using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCard
{
    public string CardID { get { return cardID; } }
    public string CardType { get { return cardType; } }
    public float GetWeight { get { return CalcWeight(); } }
    public GameObject GetObjRef { get { return cardRef; } }

    public bool HasWonGame { set { wonGame = value; } }
    public bool HasSavedPop { set { savedPop = value; } }
    public bool HasHealedUnderMax { set { growUnderMaxHealth = value; } }

    private int dmgDefended = 0;
    private int dmgDealt = 0;
    private int growthDone = 0;

    private bool wonGame = false;
    private bool savedPop = false;
    private bool growUnderMaxHealth = false;

    private string cardType;
    private string cardID;
    private GameObject cardRef;

    public TrackCard(GameObject reference, string id, string type)
    {
        cardRef = reference;
        cardID = id;
        cardType = type;
    }

    //-------------METHODS FOR DAMAGE DEALT, DEFENDED AND GROWN-------
    public void IncDmgDefended(int amount)
    {
        dmgDefended += Mathf.Abs(amount);
    }

    public void IncDmgDealt(int amount)
    {
        dmgDealt += Mathf.Abs(amount);
    }

    public void IncGrowth(int amount)
    {
        growthDone += Mathf.Abs(amount);
    }

    //-------------CALC WEIGHT-------
    private float CalcWeight()
    {
        float w = 0;

        //logic here

        //clamp so the value stays between 0 and 1
        w = Mathf.Clamp(w, 0, 1);
        return w;
    }
}
