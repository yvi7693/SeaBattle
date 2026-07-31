
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
        Instance.music.Stop();

        music = newMusic;
        Instance.music.Play();
        
    }
}