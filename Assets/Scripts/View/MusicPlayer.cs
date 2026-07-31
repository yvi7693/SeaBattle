
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    
    public static MusicPlayer Instance;

    [SerializeField] private AudioSource music;

    private void Awake()
    {
       if(Instance != null)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);  
    }

    public AudioSource GetMusic()
    {
        return music;
    }

    public void Switch(AudioSource newMusic)
    {
        if (music != null)
            music.Stop();

        music = newMusic;

        if (music != null)
            music.Play();
    }
}