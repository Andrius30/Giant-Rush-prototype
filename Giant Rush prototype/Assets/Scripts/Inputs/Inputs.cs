using UnityEngine;

public class Inputs
{
    public bool LeftInput() => Input.GetKeyDown(KeyCode.A);
    public bool RightInput() => Input.GetKeyDown(KeyCode.D);
    public bool LeftMouseInputDown() => Input.GetKeyDown(KeyCode.Mouse0);
    public bool LeftMouseInputUp() => Input.GetKeyUp(KeyCode.Mouse0);


    // TOUCH INPUTS ========================
    Vector2 fingerStart;
    float swipeDetectionDistance = 80;
    bool fingerDown;
    public bool swipeLeft;
    public bool swipeRight;
    public bool isTouching;

    public void DetectTouches()
    {
        swipeLeft = false;
        swipeRight = false;
        isTouching = false;
        if (LeftMouseInputDown() && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            fingerStart = Input.touches[0].position;
            fingerDown = true;
        }
        if (LeftMouseInputUp() && fingerDown)
        {
            fingerDown = false;
        }
        if (fingerDown)
        {
            // if (Input.touchCount <= 0) return;
            if (Input.touches[0].position.x > fingerStart.x + swipeDetectionDistance)
            {
                fingerDown = false;
                swipeRight = true;
            }
            if (Input.touches[0].position.x < fingerStart.x - swipeDetectionDistance)
            {
                fingerDown = false;
                swipeLeft = true;

            }
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                isTouching = true;
            }
            if(touch.phase == TouchPhase.Ended)
            {
                isTouching = false;
            }
        }
    }

    // =====================================
}
