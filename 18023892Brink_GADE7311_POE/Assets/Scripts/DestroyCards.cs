using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyCards : MonoBehaviour
{
    int count;
    void Start()
    {
        count = this.transform.childCount;
        
    }
    
    void Update()
    {
        count = this.transform.childCount;
        DestroyOldCards();
    }

    public void DestroyOldCards()
    {

        if (count > 7)
        {
            Debug.Log("deactivate");
            this.transform.GetChild(7).gameObject.SetActive(false);
            //Destroy(this.transform.GetChild(0));
        }
    }
}
