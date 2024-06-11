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
    private Image popup;
    private Button play;
    private Image book;
    private Button setting;
    private Button exit;

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
        if(!GameObject.Find("PausePopup").TryGetComponent<Image>(out popup))
        {
            Debug.Log("Pause - Awake - Image");
        }
        else
        {
            if(!popup.transform.Find("Play").TryGetComponent<Button>(out play))
            {
                Debug.Log("Pause - Awake - Button");
            }
            else
            {
                play.onClick.AddListener(OnPlay);
            }
            if (!popup.transform.Find("Book").TryGetComponent<Image>(out book))
            {
                Debug.Log("Pause - Awake - Image");
            }
            if (!popup.transform.Find("Setting").TryGetComponent<Button>(out setting))
            {
                Debug.Log("Pause - Awake - Button");
            }
            else
            {
                setting.onClick.AddListener(OnSetting);
            }
            if (!popup.transform.Find("Exit").TryGetComponent<Button>(out exit))
            {
                Debug.Log("Pause - Awake - Button");
            }
            else
            {
                exit.onClick.AddListener(OnExit);
            }
        }
        SetPopup(false);
    }

    private void SetPopup(bool set)
    {
        popup.gameObject.SetActive(set);
    }

    private void PressPause()
    {
        isPause = !isPause;
        if (isPause)
        {
            Time.timeScale = 0f;
            current.sprite = restartImage;
            SetPopup(true);
        }
        else
        {
            Time.timeScale = 1.0f;
            current.sprite = pauseImage;
            SetPopup(false );
        }
    }

    private void OnPlay()
    {
        isPause = false;
        Time.timeScale = 1.0f;
        current.sprite = pauseImage;
        SetPopup(false);
    }

    private void OnSetting()
    {

    }

    private void OnExit()
    {

    }
}
