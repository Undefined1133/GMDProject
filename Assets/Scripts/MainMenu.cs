using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public delegate void PlayGameQuitGameEventHandler();

    public event PlayGameQuitGameEventHandler buttonClicked;
    
    public delegate void BackEventHandler();

    public event BackEventHandler backClicked;
    public delegate void SelectEventHandler();

    public event SelectEventHandler selected;
    
    
    
    public GameObject optionsMenu;


    public void PlayGame()
    {
        buttonClicked?.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        buttonClicked?.Invoke();
        Application.Quit();
    }

    public void Select()
    {
        selected?.Invoke();
    }

    public void Options()
    {
        buttonClicked?.Invoke();
        gameObject.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void Back()
    {
        backClicked?.Invoke();
        optionsMenu.SetActive(false);
        gameObject.SetActive(true);
    }
}
