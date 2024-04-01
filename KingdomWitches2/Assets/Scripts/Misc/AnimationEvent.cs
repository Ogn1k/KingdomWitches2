using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    public UnityEvent OnAnimationEventTriggered;

    public void TriggerEvent()
    {
        OnAnimationEventTriggered?.Invoke();
    }
}
