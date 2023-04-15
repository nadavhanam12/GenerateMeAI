using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour, IEndDragHandler
{

    public ScrollRect scrollRect;
    public RectTransform content;
    public float snapSpeed = 5f;
    public float itemWidth;

    private Vector2 targetPosition;

    public void OnEndDrag(PointerEventData eventData)
    {

        int closestItemIndex = Mathf.RoundToInt(-content.anchoredPosition.x / itemWidth);
        //print(closestItemIndex);
        targetPosition = new Vector2(-closestItemIndex * itemWidth, content.anchoredPosition.y);
        //print(targetPosition);
        content.anchoredPosition = Vector2.Lerp(content.anchoredPosition, targetPosition, snapSpeed);
    }

}
