using UnityEditor; // Для доступа к Unity API для сборки
using UnityEngine; // Общие классы Unity (например, для Debug.Log)

public static class BuildScript
{
    public static void BuildIOS()
    {
        // Укажите путь для сборки
        string buildPath = "build/ios";

        // Настройка сцен для сборки
        string[] scenes = { "Assets/Scenes/MainMenuScene.unity" };

        // Выполнение сборки
        BuildPipeline.BuildPlayer(
            scenes,                // Список сцен
            buildPath,             // Путь для сборки
            BuildTarget.iOS,       // Целевая платформа
            BuildOptions.None      // Опции сборки
        );

        Debug.Log("iOS build completed successfully.");
    }
}