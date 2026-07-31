using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LoadAnimate : MonoBehaviour
{
    [SerializeField] private CanvasGroup flap;
    [SerializeField] private TMP_Text generalText;
    [SerializeField] private Slider slider;

    private void Awake()
    {
        flap.alpha = 0;
    }

    public void FlapOn()
    {
        flap.DOFade(1f, 2f)
            .OnComplete(SceneAnimateStart);
    }

    public void SceneAnimateStart()
    {
        SceneManager.LoadScene("LoadScene");
        AnimateText();
    }


    public void AnimateText()
    {
          string text = "SEABATTLE";

          generalText.text = text;
          generalText.maxVisibleCharacters = 0;

          DOTween.To(() => generalText.maxVisibleCharacters,
                    x => generalText.maxVisibleCharacters = x,
                    text.Length,
                    2f)
                    .SetEase(Ease.InOutQuad)
                    .OnComplete(AnimateSlider);

    }

    public void AnimateSlider()
    {   
        
        StartCoroutine(LoadSceneAsync());
        slider.DOValue(1f, 2f);
        
    }


    public IEnumerator LoadSceneAsync()
    {
        AsyncOperation battleScene = SceneManager.LoadSceneAsync("BattleScene");

        battleScene.allowSceneActivation = false;

        yield return new WaitUntil(() => slider.value == 1 && battleScene.progress >= 0.9);

         battleScene.allowSceneActivation = false;
    }
}