using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }
    public Staff staff { get; private set; }
    public PlayerName playerDeploy { get; private set; }
    private Mode mode;

    private void Awake()
    {
        
        staff = new Staff();

        playerDeploy = PlayerName.Player1;
        
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public Mode GetMode()
    {
        return mode;
    }

    public bool IsAiMode()
    {
        return mode == Mode.Ai;
    }

    public void SetMode(Mode mode)
    {
        this.mode = mode;
    }

    public void Next()
    {
        if(mode == Mode.Classic && playerDeploy == PlayerName.Player1)
        {
            playerDeploy = PlayerName.Player2;
            SceneManager.LoadScene("DeployScene");
        }

        else if (mode == Mode.Classic && playerDeploy == PlayerName.Player2)
            FlapAnimate.Instance.FlapOn();

        else if (mode == Mode.Ai)
        {
            staff.DeployFleet();
            FlapAnimate.Instance.FlapOn();
        }
            
    }


    public void End()
    {
        SceneManager.LoadScene("WinnerScene");
    }
}

public enum PlayerName
{
    Player1,
    Player2,
    Ai
}