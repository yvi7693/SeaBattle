using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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


    public void ShowLeftPerson(bool mood)
    {
        imageLeft.sprite = personLeft.GetIcon();
        textLeft.text = RandomMessage(personLeft, mood);

        rightGroup.gameObject.SetActive(false);
        leftGroup.gameObject.SetActive(true);
    }


    public void ShowRightPerson(bool mood)
    {
        
        imageRight.sprite = personLeft.GetIcon();
        textRight.text = RandomMessage(personRight, mood);

        leftGroup.gameObject.SetActive(false);
        rightGroup.gameObject.SetActive(true);
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