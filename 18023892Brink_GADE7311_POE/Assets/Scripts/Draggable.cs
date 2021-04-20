using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler 
{
    //hand
    public Transform parentToReturnTo = null;
    //zone
    public Transform placeholderParent = null;

    GameObject placeholder = null;

    public LayerMask whatIsCardLayer;
    public string whatIsCardTag;
    private bool isEnabled = true;

    public void Enable() { isEnabled = true; }
    public void Disable() { isEnabled = false; }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //DO NOT TOUCH!!!!
        if (!GetComponent<ThisCard>().CanBeSummoned(1) || !isEnabled)
        {
            return;
        }

        placeholder = new GameObject();
        placeholder.transform.SetParent(this.transform.parent);
        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;

        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        parentToReturnTo = this.transform.parent;
        placeholderParent = parentToReturnTo;
        this.transform.SetParent(this.transform.parent.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        //DO NOT TOUCH!!!!
        if (!GetComponent<ThisCard>().CanBeSummoned(1) || !isEnabled)
        {
            throw new CardSpecificException("Card cannot be summoned!!");
        }


        this.transform.position = eventData.position;

        if (placeholder.transform.parent != placeholderParent)
        {
            placeholder.transform.SetParent(placeholderParent);
        }

        int newSiblingIndex = placeholderParent.childCount;

        for (int i = 0; i < placeholderParent.childCount; i++)
        {
            if (this.transform.position.x < placeholderParent.GetChild(i).position.x)
            {
                newSiblingIndex = i;

                if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                {
                    newSiblingIndex--;
                }

                break;
            }
        }

        placeholder.transform.SetSiblingIndex(newSiblingIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //
        if (!GetComponent<ThisCard>().CanBeSummoned(1) || !isEnabled)
        {
            return;
        }

        //Debug.Log("End drag");
        if (whatIsCardLayer == (whatIsCardLayer | (1 << gameObject.layer)))
        {
            if (this.GetComponent<ThisCard>().CanBeSummoned(1))
            {
                this.transform.SetParent(parentToReturnTo);
                this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

                GetComponent<CanvasGroup>().blocksRaycasts = true;

                this.GetComponent<ThisCard>().Summon();

                Destroy(placeholder);
            }

            else if (this.GetComponent<ThisCard>().CanBeSummoned(2))
            {
                this.transform.SetParent(parentToReturnTo);
                this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

                GetComponent<CanvasGroup>().blocksRaycasts = true;

                this.GetComponent<ThisCard>().Summon();

                Destroy(placeholder);
            }

        }
        
    }
}
