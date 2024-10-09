using System.Diagnostics;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class LoadGameDetails : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text semesterText;
    [SerializeField] private TMP_Text titleText;

    public static string GetBasePath(string textAssetName) => Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "DemoGames" + Path.DirectorySeparatorChar + textAssetName + Path.DirectorySeparatorChar;

    public void LoadDetails(GameSelector gameSelector)
    {
        var game = gameSelector.Game;

        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(() => StartGame(GetBasePath(gameSelector.FileName) + game.GamePath));

        videoPlayer.url = new string(GetBasePath(gameSelector.FileName).Select(x => x == '\\' ? '/' : (x == '/' ? '\\' : x)).ToArray()) + gameSelector.FileName + ".mp4";
        videoPlayer.Play();

        nameText.text = game.Author;
        if (string.IsNullOrEmpty(game.SemesterAltText)) semesterText.text = $"Semester {game.Semester}";
        else semesterText.text = game.SemesterAltText;
        titleText.text = game.Title;
    }

    private void StartGame(string gamePath)
    {
        try
        {
            Process.Start(gamePath);
        }
        catch
        {
            UnityEngine.Debug.LogError($"Could not fetch executable at path {gamePath}");
        }
    }
}
