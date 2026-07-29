using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New TextComment", menuName = "TextComment", order = 51)]
public class Person: ScriptableObject
{   
    [SerializeField] private Sprite icon;
    [SerializeField] private List<String> positiveComment;
    [SerializeField] private List<String> negativeComment;


    public Sprite GetIcon()
    {
        return icon;
    }


    public List<String> GetPositiveComment()
    {
        return positiveComment;
    }


    public List<String> GetNegativeComment()
    {
        return negativeComment;
    }
    
}