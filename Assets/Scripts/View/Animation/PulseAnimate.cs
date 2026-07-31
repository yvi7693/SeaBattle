using UnityEngine;
using TMPro;
using DG.Tweening;

public class PulseAnimate : MonoBehaviour
{
    [SerializeField] private TMP_Text animateText;
    [SerializeField] private float duration = 1.2f;
    private Sequence sequence;

    [Header("Colors")]

    [SerializeField] private Color brightColor = Color.white;
    [SerializeField] private Color dimColor = new Color(0.78f, 0.9f, 1f, 0.78f);

    public void Start()
    {
        OnAnimation();
    }

    public void OnAnimation()
    {
         sequence = DOTween.Sequence()
            .Append(animateText.DOColor(dimColor, duration))
            .Append(animateText.DOColor(brightColor, duration))
            .SetEase(Ease.InOutSine)
            .SetLoops(-1)
            .SetLink(gameObject);
    }



}
