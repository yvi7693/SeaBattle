using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    public void ChooseClassicMode()
    {
        SceneManager.LoadScene("DeployScene");
    }

    public void ChooseAiMode()
    {
        
    }
}