using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    [SerializeField] private AudioSource startMusic;


    public void NewGame()
    {
        Destroy(GameSession.Instance.gameObject);
        SceneManager.LoadScene("StartScene");

        MusicPlayer.Instance.Switch(startMusic);
    }
}