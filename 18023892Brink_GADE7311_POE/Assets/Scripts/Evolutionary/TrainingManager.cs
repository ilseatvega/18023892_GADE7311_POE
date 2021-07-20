using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class TrainingManager : MonoBehaviour
{
    private TurnSystem ts;

    string trainingPath;

    List<TrackCard> vCards = new List<TrackCard>();

    //dict for cardname and weight
    Dictionary<string, float> weightDict = new Dictionary<string, float>();
    //dict for id and trainig amount
    Dictionary<string, int> amountTrained = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()
    {
        trainingPath = Application.dataPath + @"\ObjectData\TextFiles\TrainingData.txt";
        ReadData();
        ReadDebugger();
    }

    public void AddCardToVCard(GameObject reference, string cardID, string type)
    {
        foreach (TrackCard vcard in vCards)
        {
            //if we have vcard w same physical card ref
            if (vcard.GetObjRef == reference)
            {
                return;
            }
        }
        vCards.Add(new TrackCard(reference, cardID, type));
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

    public float GetTrainingAmountFromID(string cardID)
    {
        foreach (string key in amountTrained.Keys)
        {
            if (key == cardID)
            {
                return amountTrained[key];
            }
        }
        //default value if not assigned
        return 0.5f;
    }

    public int SetDamageDealt(GameObject reference, int value)
    {
        //loop through cards
        foreach (TrackCard vcard in vCards)
        {
            //if object ref of vcard is == ref taken in
            if (vcard.GetObjRef == reference)
            {
                vcard.IncDmgDealt(value);
            }
        }
        return 0;
    }
    public int SetGrowthDone(GameObject reference, int value)
    {
        //loop through cards
        foreach (TrackCard vcard in vCards)
        {
            //if object ref of vcard is == ref taken in
            if (vcard.GetObjRef == reference)
            {
                vcard.IncGrowth(value);
            }
        }
        return 0;
    }
    public int SetDmgDefended(GameObject reference, int value)
    {
        //loop through cards
        foreach (TrackCard vcard in vCards)
        {
            //if object ref of vcard is == ref taken in
            if (vcard.GetObjRef == reference)
            {
                vcard.IncDmgDefended(value);
            }
        }
        return 0;
    }

    public void SaveTrainingData()
    {
        using (StreamWriter sw = new StreamWriter(trainingPath))
        {
            sw.WriteLine("");
        }
    }

    private void ReadDebugger()
    {
        foreach (string key in weightDict.Keys)
        {
            Debug.Log($"id= {key}  weight= {weightDict[key]}  trained= {amountTrained[key]}");
        }
    }

    public void ReadData()
    {
        StreamReader sr = new StreamReader(trainingPath);

        weightDict.Clear();
        amountTrained.Clear();

        string line;
        //while lines in file
        while ((line = sr.ReadLine()) != null)
        {
            //skip these lines
            if (line[0] == '#')
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
}
