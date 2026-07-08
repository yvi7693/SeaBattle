using UnityEngine;
using TMPro;
using DG.Tweening;

public class WinnerAnimation : MonoBehaviour
{
    [SerializeField] private TMP_Text winnerText;
    [SerializeField] private TMP_Text winnerName;

    private void Start()
    {
        winnerText.alpha = 0;
        winnerName.alpha = 0;

        DOTween.Sequence()
            .Append(winnerText.DOFade(1f, 1f))
            .AppendInterval(3f)
            .Append(winnerName.DOFade(1f, 2f));
    }
}