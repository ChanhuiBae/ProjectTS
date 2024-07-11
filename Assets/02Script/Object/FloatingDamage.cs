using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingDamage : MonoBehaviour, IPoolObject
{
    private PoolManager poolManager;
    [SerializeField]
    private string name;
    private RectTransform rect;
    private TextMeshProUGUI text;
    private void Awake()
    {
        if (!GameObject.Find("PoolManager").TryGetComponent<PoolManager>(out poolManager))
        {
            Debug.Log("FloatingDamage - Awake - PoolManager");
        }
        if (!TryGetComponent<RectTransform>(out rect))
        {
            Debug.Log("FloatingDamage - Awake - RectTransform");
        }
        if(!TryGetComponent<TextMeshProUGUI>(out text))
        {
            Debug.Log("FloatingDamage - Awake - TextMeshProUGUI");
        }
    }
    public void Init(Vector3 pos,int damage)
    {
        rect.position = Camera.main.WorldToScreenPoint(pos + Vector3.up);
        text.text = damage.ToString();
        text.fontSize = 0;
        text.color = Color.white;
        StartCoroutine(Enable());
    }

    public void OnCreatedInPool()
    {
        //throw new System.NotImplementedException();
    }

    public void OnGettingFromPool()
    {
        //throw new System.NotImplementedException();
    }

    private IEnumerator Enable()
    {
        for (int i = 0; i < 25; i++)
        {
            yield return null;
            text.fontSize += 1f;
        }
        StartCoroutine(Disable());
    }

    private IEnumerator Disable()
    {
        for(int i = 0; i < 10;  i++)
        {
            yield return YieldInstructionCache.WaitForSeconds(0.01f);
            rect.position += new Vector3(0, 2f, 0);
            text.fontSize -= 1f;
            text.alpha = 1 - (0.1f * i);
        }
        poolManager.TakeToPool<FloatingDamage>(name, this);
    }
}
