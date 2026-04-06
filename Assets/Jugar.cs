using UnityEngine;
using UnityEngine.SceneManagement;

public class Jugar : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "menu2";

    public void LoadScene()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }

        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }
}
