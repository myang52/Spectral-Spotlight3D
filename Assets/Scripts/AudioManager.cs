
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip background;
    public AudioClip jump;
    public AudioClip click;
    public AudioClip coin;
    public AudioClip regainhealth;
    public AudioClip battery;
    public AudioClip ghostDeath;
    public AudioClip damage;
    public AudioClip upgrade;
    public AudioClip tooPoor;



    private void Start(){
        musicSource.clip = background;  //play background music
        musicSource.Play();

    }



    public void PlaySFX(AudioClip clip){


        SFXSource.PlayOneShot(clip);

    }
}
