using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyframeUIAnimator : MonoBehaviour
{
    public List<KeyframeConfig> configs = new List<KeyframeConfig>(); // Support multiple configs

    private void Start()
    {
        if (configs.Count == 0)
        {
            Debug.LogError("No KeyframeConfigs assigned.");
            return;
        }

        foreach (var config in configs)
        {
            if (config == null)
            {
                Debug.LogError("One of the KeyframeConfigs is null.");
                continue;
            }

            if (config.targetUIObject == null)
            {
                Debug.LogError($"Target UI Object is null in config: {config.name}");
                continue;
            }

            config.LoadKeyframes();
            StartCoroutine(AnimateUI(config)); // Start an animation coroutine for each config
        }
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

        while (index < config.keyframes.Count)
        {
            Vector3 pos = config.keyframes[index];

            config.targetUIObject.anchoredPosition = new Vector2(
                 pos.x - (canvasSize.x / 2),
                -pos.y + (canvasSize.y / 2)
            );
            index++;
            yield return new WaitForSeconds(config.updateRate);
        }
    }
}
