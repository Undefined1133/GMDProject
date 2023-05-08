using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    
    #region Singleton
    public static MenuManager instance;

    public AudioSource source;
    public AudioClip menuClickClip;
    public AudioClip menuReturnClip;
    public AudioClip menuHoverClip;
    public GameObject mainMenu;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of MenuManager found!");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        var mainMenuScript = mainMenu.GetComponent<MainMenu>();
        mainMenuScript.buttonClicked += OnButtonClicked;
        mainMenuScript.backClicked += OnBackClicked;
        mainMenuScript.selected += OnSelected;
    }

    #endregion

    private void OnButtonClicked()
    {
        source.PlayOneShot(menuClickClip);
    }

    private void OnSelected()
    {
        source.PlayOneShot(menuHoverClip);
    }

    private void OnBackClicked()
    {
        source.PlayOneShot(menuReturnClip);
    }
}
