using UnityEngine;
using System.Collections;

public class SwipeController : MonoBehaviour
{
    public float maxSwipeTime = 0.3f;
    public float minSwipeDist = 0.5f;
    public float Delay = 0.5f;

    private float TimeDelayed = 0;
    private bool isSwipe;
    private float fingerStartTime;
    private Vector2 fingerStartPos;

    public delegate void SwipeAction(int _direction);
    public static event SwipeAction OnSwipe;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        isSwipe = true;
                        fingerStartTime = Time.time;
                        fingerStartPos = touch.position;
                        break;

                    case TouchPhase.Canceled:
                        isSwipe = false;
                        break;

                    case TouchPhase.Moved:

                        float gestureTime = Time.time - fingerStartTime;
                        float gestureDist = (touch.position - fingerStartPos).magnitude;

                        if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist)
                        {
                            Vector2 direction = touch.position - fingerStartPos;
                            Vector2 swipeType = Vector2.zero;

                            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                            {
                                swipeType = Vector2.right * Mathf.Sign(direction.x);
                            }
                            else
                            {
                                swipeType = Vector2.up * Mathf.Sign(direction.y);
                            }

                            if (swipeType.x != 0.0f)
                            {
                                if (swipeType.x > 0.0f)
                                {
                                    OnSwipe(1);
                                    isSwipe = false;
                                }
                                else
                                {
                                    OnSwipe(0);
                                    isSwipe = false;
                                }
                            }

                            if (swipeType.y != 0.0f)
                            {
                                if (swipeType.y > 0.0f)
                                {
                                    OnSwipe(2);
                                    isSwipe = false;
                                }
                                else
                                {
                                    OnSwipe(3);
                                    isSwipe = false;
                                }
                            }
                        }
                        break;
                }
            }
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (h > 0 && Time.time > TimeDelayed)
        {
            OnSwipe(1);
            TimeDelayed = Time.time + Delay;
        }
        else if (h < 0 && Time.time > TimeDelayed)
        {
            OnSwipe(0);
            TimeDelayed = Time.time + Delay;
        }
        else if (v > 0 && Time.time > TimeDelayed)
        {
            OnSwipe(2);
            TimeDelayed = Time.time + Delay;
        }
        else if (v < 0 && Time.time > TimeDelayed)
        {
            OnSwipe(3);
            TimeDelayed = Time.time + Delay;
        }
    }
}