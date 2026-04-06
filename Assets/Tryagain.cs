using UnityEngine;
using UnityEngine.SceneManagement;

public class Tryagain : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "Menu";

    public void LoadScene()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
