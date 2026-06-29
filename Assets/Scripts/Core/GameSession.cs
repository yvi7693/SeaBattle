using System;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }

    public Staff staff { get; private set; }

    private void Awake()
    {
        
        staff = new Staff();
        
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }




    }