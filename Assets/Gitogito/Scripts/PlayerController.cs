using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float rotateSpeed = 1f;

    private void Update ()
    {
        float h;
#if UNITY_EDITOR
        h = HorizontalInput_Editor ();
#elif UNITY_ANDROID
        h = HorizontalInput_Android ();
#endif
        if (System.Math.Abs (h) < Mathf.Epsilon)
        {
            float z = Mathf.Round ((transform.eulerAngles.z - 15f) / 30f) * 30f + 15f;
            transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (new Vector3 (0, 0, z)), 0.5f);
        }
        else
        {
            transform.Rotate (new Vector3 (0, 0, rotateSpeed * h * Time.deltaTime));
        }
    }

    int dirXP = 0;
    float moveX = 0, deltaX;

    private float HorizontalInput_Editor ()
    {
        if (!Input.GetMouseButton(0))
        {
            dirXP = 0;
            moveX = 0;
            return 0f;
        }

        deltaX = Input.GetAxis ("Mouse X");

        return RotateDirection ();
    }

    private float HorizontalInput_Android ()
    {
        if (Input.touchCount == 0)
        {
            dirXP = 0;
            moveX = 0;
            return 0f;
        }

        Touch touch = Input.GetTouch (0);
        deltaX = touch.deltaPosition.x;

        return RotateDirection ();
    }

    private float RotateDirection ()
    {
        int dirX = PlusMinus (deltaX);
        if (dirX == dirXP || dirX == 0)
        {
            moveX += deltaX;
        }
        else
        {
            moveX = 0;
            dirXP = dirX;
        }
        return moveX;
    }

    private int PlusMinus (float f)
    {
        if (f > 0.01f)
        {
            return 1;
        }
        if (f < -0.01f)
        {
            return -1;
        }
        return 0;
    }
}
