using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }
    public Staff staff { get; private set; }
    public int playerDeploy { get; private set; }
    private Mode mode;

    private void Awake()
    {
        
        staff = new Staff();

        playerDeploy = 1;
        
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public Mode GetMode()
    {
        return mode;
    }

    public void SetMode(Mode mode)
    {
        this.mode = mode;
    }

    public void Next()
    {

        if (playerDeploy == 2)
            SceneManager.LoadScene("BattleScene");

        else if (mode == Mode.Classic)
        {
            playerDeploy = 2;
            SceneManager.LoadScene("DeployScene");
        }

        else if (mode == Mode.Ai)
            SceneManager.LoadScene("BattleScene");
        
        
    }

    public void End()
    {
        BattleController battleController = staff.GetBattleController();

        SceneManager.LoadScene("WinnerScene");
        
    }
}