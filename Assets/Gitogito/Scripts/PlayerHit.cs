using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.CompareTag ("Enemy"))
        {
            GameObject.FindWithTag("GameSystem").SendMessage ("OnHited");
        }
    }
}
