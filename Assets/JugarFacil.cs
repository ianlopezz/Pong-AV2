using UnityEngine;
using UnityEngine.SceneManagement;

public class JugarFacil : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "Game";

    public void LoadScene()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }

        GameDifficultySettings.SelectedDifficulty = GameDifficulty.Easy;
        SceneManager.LoadScene(sceneToLoad);
    }
}
