using System.Diagnostics;
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

    private void Update()
    {
        Screen.fullScreen = true;
    }

    public void LoadDetails(GameSelector gameSelector)
    {
        var game = gameSelector.Game;

        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(() => StartGame(game.GamePath));

        videoPlayer.clip = game.TrailerVideo;
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
