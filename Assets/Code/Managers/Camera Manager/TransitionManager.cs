using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    [SerializeField]
    Image bottomPanel;
    [SerializeField]
    Image topPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FadeIn(float time)
    {
        topPanel.gameObject.SetActive(true);
        bottomPanel.gameObject.SetActive(true);
        float maxLerp = topPanel.rectTransform.offsetMin.y + 10;
        float minLerp = (maxLerp - 10)/ 2.0f;
        for(float i = 0; i < time; i+= Time.deltaTime)
        {
            topPanel.rectTransform.offsetMin =
                new Vector2(topPanel.rectTransform.offsetMin.x, 
                Mathf.Lerp(minLerp, maxLerp, (time - i)/time));

            bottomPanel.rectTransform.offsetMax =
                new Vector2(bottomPanel.rectTransform.offsetMax.x, 
                -Mathf.Lerp(minLerp, maxLerp, (time-i)/time));
            yield return null;
        }

        yield return null;

    }
}
