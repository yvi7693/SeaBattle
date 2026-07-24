using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public void NewGame()
    {
        DOTween.KillAll();
        Destroy(GameSession.Instance.gameObject);
        Destroy(MusicPlayer.Instance.gameObject);

        SceneManager.LoadScene("StartScene");

       
    }
}