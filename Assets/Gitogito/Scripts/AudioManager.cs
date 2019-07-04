using UnityEngine;

public enum SoundEffect
{
    PIRO,
    PIRORO,
    PORO,
    HIT
}


public class AudioManager : MonoBehaviour
{
    public AudioClip[] sounds;
    private static AudioClip[] soundEffects = new AudioClip[4];

    private void Awake ()
    {
        for (int i = 0 ; i < 4 ; i++)
        {
            soundEffects[i] = sounds[i];
        }
        sounds.Initialize ();
    }

    public void SoundEffectPlay (SoundEffect effect)
    {
        GetComponent<AudioSource> ().PlayOneShot (soundEffects[(int)effect]);
    }

    public void BGMStop ()
    {
        GetComponent<AudioSource> ().clip = null;
    }

    public void ChangePitch (float p)
    {
        GetComponent<AudioSource> ().pitch = p;
    }
}
