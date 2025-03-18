using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyframeUIAnimator : MonoBehaviour
{
    public List<KeyframeConfig> configs = new List<KeyframeConfig>();
    private Dictionary<string, Coroutine> activeLoops = new Dictionary<string, Coroutine>();
    private HashSet<string> loopingConfigs = new HashSet<string>();


    void Awake()
    {
        foreach (var config in configs)
        {
            config.FindTargetUIObject();
        }
    }

    public KeyframeConfig CheckAndReturnConfig(string configName)
    {
        if (configs.Count == 0)
        {
            Debug.LogError("No KeyframeConfigs assigned.");
        }

        KeyframeConfig config = configs.Find(config => config.name == configName);

        if (config == null)
        {
            Debug.LogError("One of the KeyframeConfigs is null.");
        }

        if (config.targetUIObjectName == null)
        {
            Debug.LogError($"Target UI Object Name is null in config: {config.name}");
        }

        config.FindTargetUIObject();
        if (config.targetUIObject == null)
        {
            Debug.LogError($"Target UI Object couldn't be found in config: {config.name}");
        }

        return config;
    }

    public void StartAnimation(string configName)
    {
        KeyframeConfig config = CheckAndReturnConfig(configName);
        StartCoroutine(AnimateUI(config, false));
    }

    public void StartAnimationLoop(string configName)
    {
        if (loopingConfigs.Contains(configName))
        {
            Debug.Log($"Animation '{configName}' is already looping.");
            return;
        }
        loopingConfigs.Add(configName);
        KeyframeConfig config = CheckAndReturnConfig(configName);
        Coroutine loopCoroutine = StartCoroutine(AnimateUI(config, true));
        activeLoops[configName] = loopCoroutine;
    }

    public void StopAnimationLoop(string configName)
    {
        if (!loopingConfigs.Contains(configName))
        {
            Debug.Log($"Animation '{configName}' is not running.");
            return;
        }

        loopingConfigs.Remove(configName);

        if (activeLoops.TryGetValue(configName, out Coroutine loopCoroutine))
        {
            StopCoroutine(loopCoroutine);
            activeLoops.Remove(configName);
        }
    }

    IEnumerator AnimateUI(KeyframeConfig config, bool loop)
    {
        int index = 0;
        int count = 0; // for non looping animation finish

        RectTransform canvasRect = config.targetUIObject.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        if (canvasRect == null)
        {
            Debug.LogError($"No Canvas found for {config.targetUIObject.name}");
            yield break;
        }

        Vector2 canvasSize = canvasRect.sizeDelta;

        Vector2 buttonOffset = config.targetUIObject.sizeDelta;
        Vector3 currentScale = config.targetUIObject.localScale;


        while (count < config.keyframesPosition.Count || loop)
        {
            Vector3 pos = config.keyframesPosition[index];
            Vector3 scale = config.keyframesScale[index];

            config.targetUIObject.anchoredPosition = new Vector2(
                 pos.x - (canvasSize.x / 2), //- (buttonOffset.x / 2),
                -pos.y + (canvasSize.y / 2) //- (buttonOffset.y / 2)
            );
            config.targetUIObject.localScale = new Vector3(
                currentScale.x * scale.x,
                currentScale.y * scale.y,
                currentScale.z * scale.z
            );

            count++;
            index = (index + 1) % config.keyframesPosition.Count;
            yield return new WaitForSeconds(config.updateRate);
        }
    }
}
