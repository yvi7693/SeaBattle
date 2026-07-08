using UnityEngine;
using TMPro;
using DG.Tweening;

public class WinnerAnimation : MonoBehaviour
{
    [SerializeField] private TMP_Text winnerText;
    [SerializeField] private TMP_Text winnerName;
    [SerializeField] private CanvasGroup buttonCanvas;
    [SerializeField] private RectTransform buttonTransform;
    [SerializeField] private AudioSource music;


    private void Start()
    {
        MusicPlayer.Instance.Switch(music);

        winnerText.alpha = 0;
        winnerName.alpha = 0;
        buttonCanvas.alpha = 0;

        DOTween.Sequence()
            .Append(winnerText.DOFade(1f, 1f))
            .AppendInterval(2f)
            .Append(winnerName.DOFade(1f, 2f))
            .AppendInterval(1.5f)
            .Append(buttonTransform.DOAnchorPosY(-300, 2f))
            .Join(buttonCanvas.DOFade(1f, 1f));
    }
}