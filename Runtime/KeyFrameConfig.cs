using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Text.RegularExpressions;



[CreateAssetMenu(fileName = "KeyframeConfig", menuName = "KeyframeAnimation/Config", order = 1)]
public class KeyframeConfig : ScriptableObject
{
    public RectTransform targetUIObject; // Spécifique aux objets UI
    public float updateRate = 0.05f;
    public string keyframeInput;

    [HideInInspector]
    public List<Vector3> keyframes = new List<Vector3>();

    public void LoadKeyframes()
    {

        keyframes.Clear();
        if (string.IsNullOrEmpty(keyframeInput)) return;

        string[] lines = keyframeInput.Split('\n');


        foreach (string line in lines)
        {
            string cleanedLine = line.Replace("⟶", " ");
            cleanedLine = cleanedLine.TrimStart();
            cleanedLine = cleanedLine.Replace(".", ",");
            cleanedLine = Regex.Replace(cleanedLine, @"\s+", " ");


            if (string.IsNullOrWhiteSpace(cleanedLine)) continue; // Ignorer les lignes vides

            string[] values = cleanedLine.Split(' '); // Séparateur Tabulation

            if (values.Length >= 4)
            {
                if (float.TryParse(values[1], out float x) &&
                    float.TryParse(values[2], out float y) &&
                    float.TryParse(values[3], out float z))
                {
                    keyframes.Add(ConvertAEToUnity(new Vector3(x, y, 1)));
                }
            }
        }

        Debug.Log("Keyframes Loaded: " + string.Join(", ", keyframes));
    }

    private Vector3 ConvertAEToUnity(Vector3 aePosition)
    {
        if (targetUIObject == null)
        {
            Debug.LogError("Target UI Object is not set.");
            return aePosition;
        }

        RectTransform canvasRect = targetUIObject.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        if (canvasRect == null)
        {
            Debug.LogError("No Canvas found as the parent.");
            return aePosition;
        }

        Vector2 canvasSize = canvasRect.sizeDelta;
        float AE_Comp_Width = 1920f;
        float AE_Comp_Height = 1080f;

        float x = (aePosition.x / AE_Comp_Width) * canvasSize.x;
        float y = (aePosition.y / AE_Comp_Height) * canvasSize.y;
        return new Vector3(x, y, 0);
    }
}
