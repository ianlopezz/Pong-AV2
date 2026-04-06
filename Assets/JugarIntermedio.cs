using UnityEngine;
using UnityEngine.SceneManagement;

public class JugarIntermedio : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "Game";

    public void LoadScene()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }

        GameDifficultySettings.SelectedDifficulty = GameDifficulty.Medium;
        SceneManager.LoadScene(sceneToLoad);
    }
}
