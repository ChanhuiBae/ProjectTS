using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private GameObject timeBackgroun;
    private TextMeshProUGUI countdown;
    private int time;
    private TextMeshProUGUI killCount;
    private SkillButton basic;
    private SkillButton skill1;
    private SkillButton skill2;
    private SkillButton skill3;
    private UltimateButton ultimate;

    private int maxUltimateValue;
    private float currentUltimateValue;

    private Button pause;
    private Image current;
    private bool isPause;
    private Sprite pauseImage;
    private Sprite restartImage;
    private GameObject pausePopup;
    private Button play;
    private Image book;
    private Button setting;
    private Button exit;

    private GameObject settingPopup;
    private Button settingBack;
    private Slider bgm;
    private Slider sfx;
    private Toggle damageText;
    private Toggle camShake;
    private Button setSkill1;
    private Image icon1;
    private Button setSkill2;
    private Image icon2;
    private Button setSkill3;
    private Image icon3;
    private int select;

    private GameObject exitPopup;
    private Button exitButton;
    private Button notexitButton;

    private void Awake()
    {
        timeBackgroun = GameObject.Find("TimeBackground");
        if (!GameObject.Find("GameTime").TryGetComponent<TextMeshProUGUI>(out countdown))
        {
            Debug.Log("MenuManager - Awake - TextMeshProUGUI");
        }
        time = 900;
        
        if (!GameObject.Find("KillCount").TryGetComponent<TextMeshProUGUI>(out killCount))
        {
            Debug.Log("MenuManager - Awake - TextMeshProUGUI");
        }
        if (!GameObject.Find("BasicAttack").TryGetComponent<SkillButton>(out basic))
        {
            Debug.Log("PlayerController - Awake - Button");
        }

        if (!GameObject.Find("Skill1").TryGetComponent<SkillButton>(out skill1))
        {
            Debug.Log("MenuManager - Awake - SkillButton");
        }
        if (!GameObject.Find("Skill2").TryGetComponent<SkillButton>(out skill2))
        {
            Debug.Log("MenuManager - Awake - SkillButton");
        }
        if (!GameObject.Find("Skill3").TryGetComponent<SkillButton>(out skill3))
        {
            Debug.Log("MenuManager - Awake - SkillButton");
        }
        if (!GameObject.Find("Ultimate").TryGetComponent<UltimateButton>(out ultimate))
        {
            Debug.Log("MenuManager - Awake - UltimateButton");
        }


        if (!GameObject.Find("Pause").TryGetComponent<Button>(out pause))
        {
            Debug.Log("MenuManager - Awake - Button");
        }
        else
        {
            isPause = false;
            pause.onClick.AddListener(PressPause);
        }
        if (!GameObject.Find("Pause").TryGetComponent<Image>(out current))
        {
            Debug.Log("MenuManager - Awake - Image");
        }
        pauseImage = Resources.Load<Sprite>("Image/Pasue");
        restartImage = Resources.Load<Sprite>("Image/Restart");
        if (pauseImage == null || restartImage == null)
        {
            Debug.Log("MenuManager - Awake - Resources Image");
        }
        pausePopup = GameObject.Find("PausePopup");
        if (pausePopup != null)
        {
            if (!pausePopup.transform.Find("Play").TryGetComponent<Button>(out play))
            {
                Debug.Log("MenuManager - Awake - Button");
            }
            else
            {
                play.onClick.AddListener(OnPlay);
            }
            if (!pausePopup.transform.Find("Book").TryGetComponent<Image>(out book))
            {
                Debug.Log("MenuManager - Awake - Image");
            }
            if (!pausePopup.transform.Find("Setting").TryGetComponent<Button>(out setting))
            {
                Debug.Log("MenuManager - Awake - Button");
            }
            else
            {
                setting.onClick.AddListener(OnSetting);
            }
            if (!pausePopup.transform.Find("Exit").TryGetComponent<Button>(out exit))
            {
                Debug.Log("MenuManager - Awake - Button");
            }
            else
            {
                exit.onClick.AddListener(OnExit);
            }
        }
        settingPopup = GameObject.Find("SettingPopup");
        if (settingPopup != null)
        {
            if (!settingPopup.transform.Find("Back").TryGetComponent<Button>(out settingBack))
            {
                Debug.Log("MenuManager - Awake - Button");
            }
            else
            {
                settingBack.onClick.AddListener(OnSettingBack);
            }
            if(!settingPopup.transform.Find("Bgm").TryGetComponent<Slider>(out bgm))
            {
                Debug.Log("MenuManager - Awake - Slider");
            }
            else
            {
                bgm.minValue = -40;
                bgm.maxValue = 0;
                bgm.onValueChanged.AddListener(delegate { SetVolumBGM(); });
            }
            if (!settingPopup.transform.Find("Sfx").TryGetComponent<Slider>(out sfx))
            {
                Debug.Log("MenuManager - Awake - Slider");
            }
            else
            {
                sfx.minValue = -40;
                sfx.maxValue = 0;
                sfx.onValueChanged.AddListener(delegate { SetVolumSFX(); });
            }
            if(!settingPopup.transform.Find("DamageText").TryGetComponent<Toggle>(out damageText))
            {
                Debug.Log("MenuManager - Awake - Toggle");
            }
            else
            {
                damageText.onValueChanged.AddListener(delegate { SetDamageText(); });
            }
            if (!settingPopup.transform.Find("Shake").TryGetComponent<Toggle>(out camShake))
            {
                Debug.Log("MenuManager - Awake - Toggle");
            }
            else
            {
                camShake.onValueChanged.AddListener(delegate { SetCameraShake(); });
            }
            GameObject obj = settingPopup.transform.Find("SetSkill1").gameObject;
            if (!obj.TryGetComponent<Button>(out setSkill1))
            {
                Debug.Log("MenuManager - Awake - Button");
            }
            else
            {
                setSkill1.onClick.AddListener(delegate { ChangeSkillPos(1); });
            }
            if(!obj.TryGetComponent<Image>(out icon1))
            {
                Debug.Log("MenuManager - Awake - Image");
            }
            obj = settingPopup.transform.Find("SetSkill2").gameObject;
            if (!obj.TryGetComponent<Button>(out setSkill2))
            {
                Debug.Log("MenuManager - Awake - Button");
            }
            else
            {
                setSkill2.onClick.AddListener(delegate { ChangeSkillPos(2); });
            }
            if(!obj.TryGetComponent<Image>(out icon2))
            {
                Debug.Log("MenuManager - Awake - Image");
            }
            obj = settingPopup.transform.Find("SetSkill3").gameObject;
            if (!obj.TryGetComponent<Button>(out setSkill3))
            {
                Debug.Log("MenuManager - Awake - Button");
            }
            else
            {
                setSkill3.onClick.AddListener(delegate { ChangeSkillPos(3); });
            }
            if(!obj.TryGetComponent<Image>(out icon3))
            {
                Debug.Log("MenuManager - Awake - Image");
            }
        }
        exitPopup = GameObject.Find("ExitPopup");
        if (exitPopup != null)
        {
            if(!exitPopup.transform.Find("Yes").TryGetComponent<Button>(out exitButton))
            {
                Debug.Log("MenuManager - Awake - Button");
            }
            else
            {
                exitButton.onClick.AddListener(Exit);
            }
            if(!exitPopup.transform.Find("No").TryGetComponent<Button>(out notexitButton))
            {
                Debug.Log("MenuManager - Awake - Button");
            }
            else
            {
                notexitButton.onClick.AddListener(NotExit);
            }
        }

        select = 0;
        pausePopup.SetActive(false);
        settingPopup.SetActive(false);
        exitPopup.SetActive(false);
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while (time > 0)
        {
            yield return YieldInstructionCache.WaitForSeconds(1f);
            time--;
            countdown.text = (time / 60).ToString() + ":" + (time % 60).ToString();
        }
        countdown.gameObject.SetActive(false);
        timeBackgroun.SetActive(false);
    }

    public void InitSkillButton(int num, int skill, string name, int level)
    {
        switch(num)
        { 
            case 0:
                basic.Init(0, skill, name, level); 
                break;
            case 1:
                skill1.Init(1,skill, name, level);
                icon1.sprite = Resources.Load<Sprite>("Image/" + name);
                break;
            case 11:
                break;
            case 2:
                skill2.Init(2,skill, name, level);
                icon2.sprite = Resources.Load<Sprite>("Image/" + name);
                break;
            case 21:
                break;
            case 3:
                skill3.Init(3,skill, name, level);
                icon3.sprite = Resources.Load<Sprite>("Image/" + name);
                break;
            case 31:
                break;
            case 4:
                ultimate.InitSkill(4,skill, name);
                break;
            case 41:
                ultimate.InitConnectedSkill(41,skill, name);
                break;
        }
        settingPopup.SetActive(false);
    }

    public void SetMaxUltimate(int value)
    {
        maxUltimateValue = value;
        if (maxUltimateValue > currentUltimateValue)
        {
            currentUltimateValue = maxUltimateValue;
        }
        ultimate.SetUaltimate(currentUltimateValue / maxUltimateValue);
    }

    public void SetUaltimate(float value)
    {
        currentUltimateValue = value;
        ultimate.SetUaltimate(currentUltimateValue/maxUltimateValue);
    }

    public void AddUaltimate(float value)
    {
        currentUltimateValue += value;
        if(currentUltimateValue > maxUltimateValue)
        {
            currentUltimateValue = maxUltimateValue;
        }
        ultimate.SetUaltimate(currentUltimateValue/maxUltimateValue);
    }
    public void SetKillCount(int value)
    {
        killCount.text = value.ToString();
    }


    private void PressPause()
    {
        isPause = !isPause;
        if (isPause)
        {
            Time.timeScale = 0f;
            current.sprite = restartImage;
            pausePopup.SetActive(true);
        }
        else
        {
            Time.timeScale = 1.0f;
            current.sprite = pauseImage;
            pausePopup.SetActive(false);
        }
    }

    private void OnPlay()
    {
        isPause = false;
        Time.timeScale = 1.0f;
        current.sprite = pauseImage;
        pausePopup.SetActive(false);
    }

    private void OnSetting()
    {
        pausePopup.SetActive(false);
        settingPopup.SetActive(true);
        SettingData data = GameManager.Inst.GetSettinggData;
        bgm.value = data.bgm;
        sfx.value = data.sfx;
        damageText.isOn = data.damageText;
        camShake.isOn = data.cameraShake;
    }

    private void OnExit()
    {
        pausePopup.SetActive(false);
        exitPopup.SetActive(true);
    }

    private void OnSettingBack()
    {
        GameManager.Inst.SaveData();
        settingPopup.SetActive(false);
        pausePopup.SetActive(true);
    }

    private void Exit()
    {
        Time.timeScale = 1f;
        exitPopup.SetActive(false);
        // todo: player effect
        StartCoroutine(Exiting());
    }

    private IEnumerator Exiting()
    {
        yield return YieldInstructionCache.WaitForSeconds(1f);
        GameManager.Inst.AsyncLoadNextScene(SceneName.LobbyScene);
    }

    private void NotExit()
    {
        pausePopup.SetActive(true);
        exitPopup.SetActive(false);
    }
    private void SetVolumBGM()
    {
        GameManager.Inst.SetVolumBGM((int)bgm.value);
    }

    private void SetVolumSFX()
    {
        GameManager.Inst.SetVolumSFX((int)sfx.value);
    }

    private void SetDamageText()
    {
        GameManager.Inst.OnDamageText = damageText.isOn;
    }

    private void SetCameraShake()
    {
        GameManager.Inst.OnCamaraShake = camShake.isOn;
    }

    private void ChangeSkillPos(int num)
    {
        switch (select)
        {
            case 0:
                select = num;
                break;
            case 1:
                Vector3 pos = icon1.transform.position;
                Vector3 button = skill1.transform.position;
                switch (num)
                {
                    case 2:
                        icon1.transform.position = icon2.transform.position;
                        icon2.transform.position = pos;
                        skill1.transform.position = skill2.transform.position;
                        skill2.transform.position = button;
                        select = 0;
                        break;
                    case 3:
                        icon1.transform.position = icon3.transform.position;
                        icon3.transform.position = pos;
                        skill1.transform.position = skill3.transform.position;
                        skill3.transform.position = button;
                        select = 0;
                        break;
                }
                break;
            case 2:
                pos = icon2.transform.position;
                button = skill2.transform.position;
                switch (num)
                {
                    case 1:
                        icon2.transform.position = icon1.transform.position;
                        icon1.transform.position = pos;
                        skill2.transform.position = skill1.transform.position;
                        skill1.transform.position = button;
                        select = 0;
                        break;
                    case 3:
                        icon2.transform.position = icon3.transform.position;
                        icon3.transform.position = pos;
                        skill2.transform.position = skill3.transform.position;
                        skill3.transform.position = button;
                        select = 0;
                        break;
                }
                break;
            case 3:
                pos = icon3.transform.position;
                button = skill3.transform.position;
                switch (num)
                {
                    case 1:
                        icon3.transform.position = icon1.transform.position;
                        icon1.transform.position = pos;
                        skill3.transform.position = skill1.transform.position;
                        skill1.transform.position = button;
                        select = 0;
                        break;
                    case 2:
                        icon3.transform.position = icon2.transform.position;
                        icon2.transform.position = pos;
                        skill3.transform.position = skill2.transform.position;
                        skill2.transform.position = button;
                        select = 0;
                        break;
                }
                break;
        }
    }

}
