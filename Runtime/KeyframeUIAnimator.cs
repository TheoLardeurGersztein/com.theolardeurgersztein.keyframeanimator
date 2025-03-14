using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyframeUIAnimator : MonoBehaviour
{
    public List<KeyframeConfig> configs = new List<KeyframeConfig>();

    void Awake()
    {
        foreach (var config in configs)
        {
            config.FindTargetUIObject();
        }
    }

    public void StartAnimation(string configName)
    {

        if (configs.Count == 0)
        {
            Debug.LogError("No KeyframeConfigs assigned.");
            return;
        }

        KeyframeConfig config = configs.Find(config => config.name == configName);


        //foreach (var config in configs)

        if (config == null)
        {
            Debug.LogError("One of the KeyframeConfigs is null.");
        }

        if (config.targetUIObject == null)
        {
            Debug.LogError($"Target UI Object is null in config: {config.name}");
        }

        config.FindTargetUIObject();

        if (config.targetUIObject == null)
        {
            Debug.LogError($"Target UI Object is null in config: {config.name}");
            return;
        }

        config.LoadKeyframes();
        StartCoroutine(AnimateUI(config)); // Start an animation coroutine for each config

    }

    IEnumerator AnimateUI(KeyframeConfig config)
    {
        int index = 0;
        RectTransform canvasRect = config.targetUIObject.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        if (canvasRect == null)
        {
            Debug.LogError($"No Canvas found for {config.targetUIObject.name}");
            yield break;
        }

        Vector2 canvasSize = canvasRect.sizeDelta;

        Vector2 localPosition = config.targetUIObject.anchoredPosition;
        Vector3 currentScale = config.targetUIObject.localScale;


        while (index < config.keyframesPosition.Count)
        {
            Vector3 pos = config.keyframesPosition[index];
            Vector3 scale = config.keyframesScale[index];

            config.targetUIObject.anchoredPosition = new Vector2(
                 pos.x - (canvasSize.x / 2),
                -pos.y + (canvasSize.y / 2)
            );
            config.targetUIObject.localScale = new Vector3(
                currentScale.x * scale.x,
                currentScale.y * scale.y,
                currentScale.z * scale.z
            );

            index++;
            yield return new WaitForSeconds(config.updateRate);
        }
    }
}
