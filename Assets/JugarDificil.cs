using UnityEngine;
using UnityEngine.SceneManagement;

public class JugarDificil : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "Game";

    public void LoadScene()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }

        GameDifficultySettings.SelectedDifficulty = GameDifficulty.Hard;
        SceneManager.LoadScene(sceneToLoad);
    }
}
