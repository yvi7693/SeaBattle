using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LightAnimate : MonoBehaviour
{

    [SerializeField] private TMP_Text textName;
    [SerializeField] private TMP_Text textPresent;
 

    public void Start()
    {
        textName.alpha = 0f;
        textPresent.alpha = 0f;

        DOTween.Sequence()
            .AppendInterval(0.2f)
            .Append(textName.DOFade(1f, 1f))
            .AppendInterval(0.6f)
            .Append(textPresent.DOFade(1f, 1f))
            .AppendInterval(1f)
            .OnComplete(() =>
            {
                SceneManager.LoadScene("StartScene");
            });
    }
}