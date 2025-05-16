using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject optionsMenu;


    public void CloseMainMenu()
    {
        mainMenu.SetActive(false);
    }
    public void LoadScene(int sceneIndex)
    {
        GameDataPersistenceManager.Instance.CreateNewData();
        GameDataPersistenceManager.Instance.SaveTheData();
        SceneManager.LoadScene(sceneIndex);
    }
    public void ShowMainMenu()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void ShowOptionsMenu() 
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    public void ResetDestroyableObjectsSet()
    {
        ObjectTracker.DestroyedObjectIDs.Clear();
    } 
    public void ExitApplication()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
