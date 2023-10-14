using UnityEngine;
using UnityEngine.EventSystems;

public class ResizeButtonOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 _hoverSize = new Vector3(1.08f, 1.08f, 1.08f);

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = _hoverSize;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }
}