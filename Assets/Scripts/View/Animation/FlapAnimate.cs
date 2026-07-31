
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlapAnimate : MonoBehaviour
{
    public static FlapAnimate Instance {get; private set;}
    [SerializeField] private CanvasGroup flap;

    private void Awake()
    {
        Instance = this;
        flap.alpha = 0;
        flap.gameObject.SetActive(false);
    }
    

    public void FlapOn()
    {
        flap.gameObject.SetActive(true);

        DOTween.Sequence()
            .Append(flap.DOFade(1f, 1.2f))
            .Join(MusicPlayer.Instance.GetMusic().DOFade(0f, 1.2f))
            .OnComplete(SceneAnimateStart)
            .SetLink(gameObject);
    }

    public void SceneAnimateStart()
    {
        SceneManager.LoadScene("LoadScene");
    }
}