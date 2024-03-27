using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    private Button pause;
    private Image current;
    private bool isPause;
    private Sprite pauseImage;
    private Sprite restartImage;

    private void Awake()
    {
        if (!TryGetComponent<Button>(out pause))
        {
            Debug.Log("Pause - Awake - Button");
        }
        else
        {
            isPause = false;
            pause.onClick.AddListener(PressPause);
        }
        if(!TryGetComponent<Image>(out current))
        {
            Debug.Log("Pause - Awake - Image");
        }
        pauseImage = Resources.Load<Sprite>("Image/Pasue");
        restartImage = Resources.Load<Sprite>("Image/Restart");
        if(pauseImage == null || restartImage == null)
        {
            Debug.Log("Pause - Awake - Resources Image");
        }
    }

    private void PressPause()
    {
        isPause = !isPause;
        if (isPause)
        {
            Time.timeScale = 0f;
            current.sprite = restartImage;
        }
        else
        {
            Time.timeScale = 1.0f;
            current.sprite = pauseImage;
        }
    }
}
