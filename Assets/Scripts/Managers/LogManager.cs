using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    #region Singleton

    public static LogManager instance;

    public GameObject errorTextPrefab;
    public GameObject goldGainedTextPrefab;
    public GameObject expGainedTextPrefab;
    private float destroyTime = 2f;
    private int maxLogCount = 4;
    private List<GameObject> logList = new();

    public GridLayoutGroup logGrid;

    #endregion
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of LogManager found!");
            return;
        }
        instance = this;
    }
    public void ErrorLog(string message)
    {
        if (logList.Count >= maxLogCount)
        {
            logList.Remove(logList[0]);
        }
        
        GameObject newText = Instantiate(errorTextPrefab, logGrid.transform);
        newText.GetComponent<TextMeshProUGUI>().text = message;
        logList.Add(newText);
        Destroy(newText, destroyTime);
    }
    public void GoldGainedLog(string message)
    {
        if (logList.Count >= maxLogCount)
        {
            logList.Remove(logList[0]);
        }
        
        GameObject newText = Instantiate(goldGainedTextPrefab, logGrid.transform);
        newText.GetComponent<TextMeshProUGUI>().text = message;
        logList.Add(newText);
        Destroy(newText, destroyTime);
    }
    public void ExpGainedLog(string message)
    {
        if (logList.Count >= maxLogCount)
        {
            logList.Remove(logList[0]);
        }
        
        GameObject newText = Instantiate(expGainedTextPrefab, logGrid.transform);
        newText.GetComponent<TextMeshProUGUI>().text = message;
        logList.Add(newText);
        Destroy(newText, destroyTime);
    }

    private void OnDestroy()
    {
        if (logList.Count != 0 && logList[0] != null)
        {
            logList.Remove(logList[0]);
        }
    }

}
