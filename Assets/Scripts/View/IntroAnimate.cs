using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class IntroAnimate : MonoBehaviour
{

    [SerializeField] private TMP_Text textName;
    [SerializeField] private RectTransform textTransform;
    [SerializeField] private TMP_Text textPresent;
    [SerializeField] private AudioSource introAudio;
 

    public void Start()
    {
        textName.alpha = 0f;
        textPresent.alpha = 0f;

        DOTween.Sequence()
            .AppendInterval(0.2f)
            .Append(textName.DOFade(1f, 1f))
            .Append(textTransform.DOScale(1.1f, 0.1f))
            .JoinCallback(() => introAudio.Play())
            .Append(textTransform.DOScale(1f, 0.1f))
            .AppendInterval(0.5f)
            .Join(textPresent.DOFade(1f, 2f))
            .Append(introAudio.DOFade(0f, 1f))
            .AppendInterval(1f)
            .OnComplete(() =>
            {
                SceneManager.LoadScene("StartScene");
            });
    }
}