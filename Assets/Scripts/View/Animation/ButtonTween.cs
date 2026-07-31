using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float hoverScale = 1.08f;
    [SerializeField] private float duration = 0.12f;
    private Vector3 defoultScale;

    private void Awake()
    {
        defoultScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(defoultScale * hoverScale, duration)
                .SetEase(Ease.OutBack).
                SetLink(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(defoultScale, duration).SetEase(Ease.OutQuad).SetLink(gameObject);
    }
}