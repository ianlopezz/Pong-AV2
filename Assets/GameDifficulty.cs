public enum GameDifficulty
{
    Easy,
    Medium,
    Hard
}

public static class GameDifficultySettings
{
    public static GameDifficulty SelectedDifficulty { get; set; } = GameDifficulty.Medium;
}
