using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameSelector : MonoBehaviour
{
    [SerializeField] private TextAsset game;
    public Game Game => (Game)JsonUtility.FromJson(game.text, typeof(Game));
    public string FileName => game.name;

    private void Start()
    {
        if (string.IsNullOrWhiteSpace(Game.ThumbnailPath)) return;
        GetComponent<Image>().sprite = LoadSpriteFromFile(LoadGameDetails.GetBasePath(FileName) + Game.ThumbnailPath);
    }

    [ContextMenu("Generate file")]
    public void SaveToJson()
    {
        Debug.Log(Path.DirectorySeparatorChar);
        Debug.Log(Path.PathSeparator);
        Debug.Log(Path.VolumeSeparatorChar);
        Debug.Log(Path.AltDirectorySeparatorChar);
        return;
        string filePath = Path.Combine(Application.persistentDataPath, "test.json");
        Debug.Log($"saved to {Application.persistentDataPath}");
        File.WriteAllText(filePath, JsonUtility.ToJson(new Game()
        {
            Author = "Test",
            Semester = 1,
            SemesterAltText = "Test",
            GamePath = filePath,
            ThumbnailPath = "Test",
            Title = "Test",
            TrailerVideoPath = "Test",
        }, true));
    }

    private Sprite LoadSpriteFromFile(string filePath)
    {
        if (!File.Exists(filePath)) return null;

        var texture = new Texture2D(2, 2);
        var fileData = File.ReadAllBytes(filePath);

        texture.LoadImage(fileData);

        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));

        return sprite;
    }
}
