using UnityEngine;
using DG.Tweening;
using TMPro;

public class StartAnimate : MonoBehaviour
{
    [SerializeField] private TMP_Text mainText;
    [SerializeField] private RectTransform mainTransform;
    [SerializeField] private CanvasGroup groupButton;
    [SerializeField] private RectTransform buttonLeft;
    [SerializeField] private RectTransform buttonRight;

    public void Start()
    {
        mainText.alpha = 0f;
        groupButton.alpha = 0f;

        DOTween.Sequence()
            .SetDelay(0.5f)
            .Append(mainText.DOFade(1f, 3f))
            .Join(mainTransform.DOScale(1.1f, 3f))
            .SetEase(Ease.OutBack)
            .SetDelay(1f)
            .Append(buttonLeft.DOAnchorPosY(-300, 2f))
            .Join(buttonRight.DOAnchorPosY(-300, 2f))
            .Join(groupButton.DOFade(1f, 2f))
            .SetEase(Ease.OutBack)
            .SetLink(gameObject);

    }

}
