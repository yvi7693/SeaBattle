using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CommentaryView : MonoBehaviour
{
    [SerializeField] private CanvasGroup leftGroup;
    [SerializeField] private CanvasGroup rightGroup;

    [SerializeField] private Person personLeft;
    [SerializeField] private Person personRight;

    [SerializeField] private Image imageLeft;
    [SerializeField] private Image imageRight;

    [SerializeField] private TMP_Text textLeft;
    [SerializeField] private TMP_Text textRight;

    public void Awake()
    {
        leftGroup.alpha = 0;
        rightGroup.alpha = 0;
    }


    public void ShowLeftPerson(bool mood)
    {
        imageLeft.sprite = personLeft.GetIcon();
        textLeft.text = RandomMessage(personLeft, mood);

        Animate(leftGroup);
    }


    public void ShowRightPerson(bool mood)
    {
        imageRight.sprite = personRight.GetIcon();
        textRight.text = RandomMessage(personRight, mood);

        Animate(rightGroup);
    }


    public void Animate(CanvasGroup group)
    {
        group.gameObject.SetActive(true);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(group.DOFade(1f, 0.5f).SetEase(Ease.InOutSine));
        sequence.AppendInterval(1.5f);
        sequence.Append(group.DOFade(0f, 0.5f).SetEase(Ease.InOutSine));
        sequence.OnComplete(() =>
            group.gameObject.SetActive(false));

    }

    public string RandomMessage(Person person, bool positive)
    {
        List<String> comments = new List<String>();

        if (positive)
            comments = person.GetPositiveComment();
        
        else
            comments = person.GetNegativeComment();
        
        System.Random random = new System.Random();

        int randomIndex = random.Next(0, comments.Count);

        return comments[randomIndex];

    }
}