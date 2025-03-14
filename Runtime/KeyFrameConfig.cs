using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Text.RegularExpressions;



[CreateAssetMenu(fileName = "KeyframeConfig", menuName = "KeyframeAnimation/Config", order = 1)]
public class KeyframeConfig : ScriptableObject
{
    public RectTransform targetUIObject;
    [SerializeField] public string targetUIObjectName;
    public float updateRate = 0.05f;
    public string keyframeInput;

    [HideInInspector]
    public List<Vector3> keyframesPosition = new List<Vector3>();
    [HideInInspector]
    public List<Vector3> keyframesScale = new List<Vector3>();

    public void LoadKeyframes()
    {

        if (targetUIObject == null)
        {
            targetUIObjectName = "";
            return;
        }
        else
        {
            targetUIObjectName = targetUIObject.name;
        }

        if (string.IsNullOrEmpty(keyframeInput)) return;

        string scalePattern = @"(?<=Transform\sScale)(.*?)(?=\s*Transform|$)";
        string positionPattern = @"(?<=Transform\sPosition)(.*?)(?=\s*Transform|$)";



        // Extract Scale data
        keyframesScale.Clear();
        Match scaleMatch = Regex.Match(keyframeInput, scalePattern, RegexOptions.Singleline);
        string scaleData = scaleMatch.Value.Trim();
        scaleData = Regex.Replace(scaleData, @"^\s*Frame\s+X\s+percent\s+Y\s+percent\s+Z\s+percent\s*$", "", RegexOptions.Multiline).Trim();

        string[] scales = scaleData.Split('\n');

        foreach (string line in scales)
        {
            string cleanedLine = line.Replace("⟶", " ");
            cleanedLine = cleanedLine.TrimStart();
            cleanedLine = cleanedLine.Replace(".", ",");
            cleanedLine = Regex.Replace(cleanedLine, @"\s+", " ");


            if (string.IsNullOrWhiteSpace(cleanedLine)) continue; // Ignorer les lignes vides

            string[] values = cleanedLine.Split(' ');
            if (values.Length >= 4)
            {
                if (float.TryParse(values[1], out float x) &&
                    float.TryParse(values[2], out float y) &&
                    float.TryParse(values[3], out float z))
                {
                    keyframesScale.Add(new Vector3(x / 100, y / 100, 1));

                }
            }
        }

        //Debug.Log("KeyframesScale Loaded: " + string.Join(", ", keyframesScale));


        keyframesPosition.Clear();
        Match positionMatch = Regex.Match(keyframeInput, positionPattern, RegexOptions.Singleline);
        string positionData = positionMatch.Value.Trim();
        positionData = Regex.Replace(positionData, @"^\s*Frame\s+X\s+percent\s+Y\s+percent\s+Z\s+percent\s*$", "", RegexOptions.Multiline).Trim();

        string[] positions = positionData.Split('\n');

        foreach (string line in positions)
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
                    keyframesPosition.Add(ConvertAEToUnity(new Vector3(x, y, 1)));
                }
            }
        }

        //Debug.Log("Keyframes Loaded: " + string.Join(", ", keyframesPosition));
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

    public void FindTargetUIObject()
    {
        if (!string.IsNullOrEmpty(targetUIObjectName))
        {
            GameObject foundObject = GameObject.Find(targetUIObjectName);
            if (foundObject != null)
            {
                targetUIObject = foundObject.GetComponent<RectTransform>();
            }
            else
            {
                Debug.LogWarning($"Target UI Object '{targetUIObjectName}' not found in scene.");
            }
        }
    }

}
