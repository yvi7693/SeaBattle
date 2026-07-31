using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LoadAnimate : MonoBehaviour
{
    [SerializeField] private TMP_Text generalText;
    [SerializeField] private Slider slider;


    private void Awake()
    {
        generalText.maxVisibleCharacters = 0;
        slider.gameObject.SetActive(false);
    }


    private void Start()
    {
        AnimateText();
    }


    public void AnimateText()
    {
          string text = "SEABATTLE";

          generalText.text = text;
         
          DOTween.To(() => generalText.maxVisibleCharacters,
                    x => generalText.maxVisibleCharacters = x,
                    text.Length,
                    2f)
                    .SetEase(Ease.InOutQuad)
                    .OnComplete(AnimateSlider)
                    .SetLink(gameObject);

    }


    public void AnimateSlider()
    {   
        
        StartCoroutine(LoadSceneAsync());

        slider.gameObject.SetActive(true);
        slider.DOValue(1f, 2f).SetLink(gameObject);
        
    }


    public IEnumerator LoadSceneAsync()
    {
        AsyncOperation battleScene = SceneManager.LoadSceneAsync("BattleScene");

        battleScene.allowSceneActivation = false;

        yield return new WaitUntil(() => slider.value == 1f && battleScene.progress >= 0.9f);

         battleScene.allowSceneActivation = true;
    }
}