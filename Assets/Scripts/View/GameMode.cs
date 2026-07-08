using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
   

    public void ChooseClassicMode()
    {
        GameSession.Instance.SetMode(Mode.Classic);
        SceneManager.LoadScene("DeployScene");
    }

    public void ChooseAiMode()
    {
        GameSession.Instance.SetMode(Mode.Ai);
        SceneManager.LoadScene("DeployScene");
    }

    
}

public enum Mode
{
    Classic,
    Ai
}
