using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCard
{
    const int STARTING_HP = 100;

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
        if (cardType == "Attack")
        {
            //80/20 ratio - damage is 80% and whether card won game is 20%
            w += (.8f / STARTING_HP) * dmgDealt;
            if (wonGame == true)
            {
                w += .2f;
            }
        }
        else if (cardType == "Defence")
        {
            //80/20 ratio - damage defended & whether this card saved a population
            w += (.8f / STARTING_HP) * dmgDefended;
            if (savedPop == true)
            {
                w += .2f;
            }
        }
        else
        {
            //80/20 ratio - growth done & whether card added growth while population was under 100 health
            w += (.8f / STARTING_HP) * growthDone;
            if (growUnderMaxHealth == true)
            {
                w += .2f;
            }
        }

        //clamp so the value stays between 0 and 1
        w = Mathf.Clamp(w, 0, 1);
        return w;
    }
}
