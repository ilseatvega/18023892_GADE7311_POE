using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class TrainingManager : MonoBehaviour
{
    private TurnSystem ts;
    private AI_Module ai;

    string trainingPath;

    List<TrackCard> tCards = new List<TrackCard>();

    //dict for cardname and weight
    Dictionary<string, float> weightDict = new Dictionary<string, float>();
    //dict for id and trainig amount - training amount needed for average
    Dictionary<string, int> amountTrained = new Dictionary<string, int>();
    //weights of cards for this game (since there are duplicates)
    Dictionary<string, float> runtimeWeights = new Dictionary<string, float>();

    public bool hasWrittenToFile { set { hasSavedData = value; } }
    private bool hasSavedData = false;

    // Start is called before the first frame update
    void Start()
    {
        trainingPath = Application.dataPath + @"\ObjectData\TextFiles\TrainingData.txt";
        ai = GameObject.FindGameObjectWithTag("AI").GetComponent<AI_Module>();
        StartCoroutine(WaitForCards());
        ReadData();
        //ReadDebugger();
    }

    public void AddCardToTCard(GameObject reference, string cardID, string type)
    {
        foreach (TrackCard tcard in tCards)
        {
            //if we have tcard w same physical card ref
            if (tcard.GetObjRef == reference)
            {
                return;
            }
        }
        tCards.Add(new TrackCard(reference, cardID, type));
    }

    public void RemoveCardFromTCard(GameObject reference)
    {
        foreach (TrackCard tcard in tCards)
        {
            //specific cards
            if (tcard.GetObjRef == reference)
            {
                //average values of all duplicates played in this game
                float newWeight = averageOfDuplicates(tcard.CardID, tcard.GetWeight);
                Debug.Log("new Calc Weight for Card: " + tcard.CardID + " is " + newWeight);
                //
                if (runtimeWeights.ContainsKey(tcard.CardID))
                {
                    runtimeWeights[tcard.CardID] = newWeight;
                }
                else
                {
                    runtimeWeights.Add(tcard.CardID, newWeight);
                }
                //removing card when game ends
                tCards.Remove(tcard);
                return;
            }
        }
    }

    private float averageOfDuplicates(string cardID, float newWeight)
    {
        float w = 0;

        if (runtimeWeights.ContainsKey(cardID))
        {
            w = (runtimeWeights[cardID] + newWeight) / 2;
        }
        else
        {
            w = newWeight;
        }

        return w;
    }

    public float GetWeightFromID(string cardID)
    {
        foreach (string key in weightDict.Keys)
        {
            if (key == cardID)
            {
                return weightDict[key];
            }
        }
        return 0.5f;
    }

    public HighestCard findHighestWeight(List<Transform> validCards)
    {
        Transform cardToPlay = default(Transform);

        float[] weights = new float[validCards.Count];
        for (int i = 0; i < validCards.Count; i++)
        {
            weights[i] = GetWeightFromID(validCards[i].GetComponent<ThisCard>().cardname);
        }

        float temp = 0;
        int tempIdx = 0;

        for (int i = 0; i < weights.Length; i++)
        {
            if (weights[i] >= temp)
            {
                temp = weights[i];
                cardToPlay = validCards[i];
                tempIdx = i;
            }
        }
        HighestCard returnCard = new HighestCard();
        returnCard.cardTrans = cardToPlay;
        returnCard.cardIndex = tempIdx;
        return returnCard;

    }

    private float GetTrainingAmountFromID(string cardID)
    {
        foreach (string key in amountTrained.Keys)
        {
            if (key == cardID)
            {
                return amountTrained[key];
            }
        }
        //default value if not assigned
        return 0;
    }

    public void SetDamageDealt(GameObject reference, int value)
    {
        //loop through cards
        foreach (TrackCard tcard in tCards)
        {
            if (tcard.GetObjRef == reference)
            {
                tcard.IncDmgDealt(value);
            }
        }
    }
    public void SetGrowthDone(GameObject reference, int value)
    {
        //loop through cards
        foreach (TrackCard tcard in tCards)
        {
            if (tcard.GetObjRef == reference)
            {
                tcard.IncGrowth(value);
            }
        }
    }
    public void SetDmgDefended(GameObject reference, int value)
    {
        //loop through cards
        foreach (TrackCard tcard in tCards)
        {
            if (tcard.GetObjRef == reference)
            {
                tcard.IncDmgDefended(value);
            }
        }
    }
    //---------------BOOLS THAT CAN ADD WEIGHT
    public void WonTheGame(GameObject reference, bool won)
    {
        //loop through cards
        foreach (TrackCard tcard in tCards)
        {
            if (tcard.GetObjRef == reference)
            {
                tcard.HasWonGame = won;
            }
        }
    }
    public void GrowUnderHealth(GameObject reference, bool grow)
    {
        //loop through cards
        foreach (TrackCard tcard in tCards)
        {
            if (tcard.GetObjRef == reference)
            {
                tcard.HasHealedUnderMax = grow;
            }
        }
    }
    public void SavedPop(GameObject reference, bool saved)
    {
        //loop through cards
        foreach (TrackCard tcard in tCards)
        {
            if (tcard.GetObjRef == reference)
            {
                tcard.HasSavedPop = saved;
            }
        }
    }

    public void SaveTrainingData()
    {
        if (hasSavedData)
        {
            return;
        }
        hasSavedData = true;

        Debug.Log("SAVING TRAINING TO FILE");
        Dictionary<string, float> newWeights = new Dictionary<string, float>();
        GenerateNewWeights(out newWeights);

        //deletes everything in file
        File.WriteAllText(trainingPath, "");

        //update values in streamwriter
        StreamWriter sw = new StreamWriter(trainingPath);
        sw.WriteLine("/format is: cardName~weight~amountTrained");
        sw.WriteLine("/");

        foreach (string cardID in newWeights.Keys)
        {
            sw.WriteLine(cardID + "~" + newWeights[cardID] + "~" + amountTrained[cardID]);
        }

        sw.Close();
    }

    private void GenerateNewWeights(out Dictionary<string, float> target)
    {
        target = new Dictionary<string, float>();
        target = weightDict;

        foreach (string cardID in runtimeWeights.Keys)
        {
            //if card has been trained
            if (weightDict.ContainsKey(cardID))
            {
                float newWeight = ((weightDict[cardID] * amountTrained[cardID]) + runtimeWeights[cardID]) / (amountTrained[cardID] + 1);
                amountTrained[cardID] += 1;
            }
            else
            {
                target.Add(cardID, runtimeWeights[cardID]);
                amountTrained.Add(cardID, 1);
            }
        }
    }

    private void ReadDebugger()
    {
        foreach (string key in weightDict.Keys)
        {
            Debug.Log("id= " + key + "weight= " + weightDict[key]  + "trained= " + amountTrained[key]);
        }
    }

    private void ReadData()
    {
        StreamReader sr = new StreamReader(trainingPath);

        weightDict.Clear();
        amountTrained.Clear();

        string line;
        //while lines in file
        while ((line = sr.ReadLine()) != null)
        {
            //skip these lines
            if (line[0] == '/')
            {
                continue;
            }
            else
            {
                string[] lineChonk = line.Split('~');

                //0= id, 1= weight, 2= trained times
                float fWeight;
                float.TryParse(lineChonk[1], out fWeight);
                weightDict.Add(lineChonk[0], fWeight);

                int iCount;
                int.TryParse(lineChonk[2], out iCount);
                amountTrained.Add(lineChonk[0], iCount);
            }
        }

        sr.Close();
    }

    IEnumerator WaitForCards()
    {
        yield return new WaitForSeconds(4.6f);
        ai.enabled = true;
    }
}

public struct HighestCard
{
    public Transform cardTrans;
    public int cardIndex;

}

