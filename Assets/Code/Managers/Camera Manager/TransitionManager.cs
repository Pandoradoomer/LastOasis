using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    [SerializeField]
    Image bottomPanel;
    [SerializeField]
    Image topPanel;

    [SerializeField]
    float fadeOutTime;
    [SerializeField]
    float fadeInTime;
    [SerializeField]
    GameObject inventory;
    [SerializeField]
    GameObject playerStats;
    // Start is called before the first frame update
    public static TransitionManager Instance;
    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
        SceneManager.sceneUnloaded += OnLevelUnloaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
        SceneManager.sceneUnloaded -= OnLevelUnloaded;
    }
    void Start()
    {

    }


    public void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Ship")
        {
            PlayerStats.Instance.ResetDeath();
        }
        StartCoroutine(FadeOut());
    }

    public void OnLevelUnloaded(Scene scene)
    {
        //Singleton.Reset();
    }

    public IEnumerator FadeOut()
    {
        topPanel.gameObject.SetActive(true);
        bottomPanel.gameObject.SetActive(true);
        EventManager.TriggerEvent(Event.DialogueFinish, null);
        float minLerp = topPanel.rectTransform.offsetMin.y;
        float maxLerp = minLerp * 2.0f;
        for (float i = 0; i < 0.1f; i += Time.deltaTime)
            yield return null;
        EventManager.TriggerEvent(Event.DialogueStart, null);
        for (float i = 0; i < fadeOutTime; i += Time.deltaTime)
        {
            topPanel.rectTransform.offsetMin =
                new Vector2(topPanel.rectTransform.offsetMin.x,
                Mathf.Lerp(minLerp, maxLerp, i /fadeOutTime));

            bottomPanel.rectTransform.offsetMax =
                new Vector2(bottomPanel.rectTransform.offsetMax.x,
                -Mathf.Lerp(minLerp, maxLerp, i /fadeOutTime));
            yield return null;
        }
        yield return null;
        EventManager.TriggerEvent(Event.DialogueFinish, null);
    }

    public IEnumerator FadeIn()
    {
        EventManager.TriggerEvent(Event.DialogueStart, new StartDialoguePacket());
        PlayerStats.Instance.SaveValues();
        topPanel.gameObject.SetActive(true);
        bottomPanel.gameObject.SetActive(true);
        float maxLerp = topPanel.rectTransform.offsetMin.y + 10;
        float minLerp = (maxLerp - 10)/ 2.0f;
        for(float i = 0; i < fadeInTime; i+= Time.deltaTime)
        {
            topPanel.rectTransform.offsetMin =
                new Vector2(topPanel.rectTransform.offsetMin.x, 
                Mathf.Lerp(minLerp, maxLerp, (fadeInTime - i)/fadeInTime));

            bottomPanel.rectTransform.offsetMax =
                new Vector2(bottomPanel.rectTransform.offsetMax.x, 
                -Mathf.Lerp(minLerp, maxLerp, (fadeInTime - i)/fadeInTime));
            yield return null;
        }
        yield return null;

    }
}
