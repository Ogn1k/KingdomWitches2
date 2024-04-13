using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HeartStatus
{
    Empty = 0,
    Half = 1,
    Full = 2
};

public class hearts : MonoBehaviour
{
    public Texture fullHeart, halfHeart, emptyHeart;
    RawImage heartImage;
    private void Awake()
    {
        heartImage = GetComponent<RawImage>();
    }

    public void SetHeartImage(HeartStatus status)
    {
        switch (status) 
        { 
            case HeartStatus.Full:
                heartImage.texture = fullHeart;
                break;
            case HeartStatus.Half:
                heartImage.texture = halfHeart;
                break;
            case HeartStatus.Empty:
                heartImage.texture = emptyHeart;
                break;
        }
    }
}
