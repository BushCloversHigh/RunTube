using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.CompareTag ("Enemy"))
        {
            GameObject.FindWithTag("GameSystem").SendMessage ("OnHited");
            AudioManager audioManager = GameObject.FindWithTag ("Audio").GetComponent<AudioManager> ();
            audioManager.ChangePitch (1f);
            audioManager.SoundEffectPlay (SoundEffect.HIT);
        }
    }
}
