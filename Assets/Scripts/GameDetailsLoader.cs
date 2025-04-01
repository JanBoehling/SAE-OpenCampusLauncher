using System.Diagnostics;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Debug = UnityEngine.Debug;

public class GameDetailsLoader : MonoBehaviour
{
    [SerializeField] private GameObject gameDetailsContainer;
    [SerializeField] private Button startButton;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage videoPlayerImage;
    [SerializeField] private RenderTexture videoRenderTexture;
    [SerializeField] private Texture loadingVideoTexture;
    [SerializeField] private Texture noPreviewTexture;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text semesterText;
    [SerializeField] private TMP_Text titleText;

    private void Start()
    {
        videoPlayer.prepareCompleted += PlayVideo;
        videoPlayer.errorReceived += (_, _) => videoPlayerImage.texture = noPreviewTexture;
    }

    private void OnDestroy()
    {
        videoPlayer.prepareCompleted -= PlayVideo;
        videoPlayer.errorReceived -= (_, _) => videoPlayerImage.texture = noPreviewTexture;
    }

    public static string GetBasePath(string textAssetName) => Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "DemoGames" + Path.DirectorySeparatorChar + textAssetName + Path.DirectorySeparatorChar;

    [System.Obsolete("GameSelector class is not in use anymore. Use instead: LoadDetails(Game)")]
    public void LoadDetails(GameSelector gameSelector)
    {
        gameDetailsContainer.SetActive(true);

        var game = gameSelector.Game;

        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(() => StartGame(GetBasePath(gameSelector.FileName) + game.GamePath));

        string videoUrl = new string(GetBasePath(gameSelector.FileName).Select(x => x == '\\' ? '/' : (x == '/' ? '\\' : x)).ToArray()) + gameSelector.FileName + ".mp4";
        Debug.Log(videoUrl);
        videoPlayer.url = videoUrl;
        videoPlayer.Play();

        nameText.text = game.Author;
        if (string.IsNullOrEmpty(game.SemesterAltText)) semesterText.text = $"Semester {game.Semester}";
        else semesterText.text = game.SemesterAltText;
        titleText.text = game.Title;
    }

    public void LoadDetails(Game game)
    {
        gameDetailsContainer.SetActive(true);

        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(() => StartGame(game.GamePath));

        videoPlayerImage.texture = loadingVideoTexture;

        string videoUrl = new string(game.TrailerVideoPath.Select(x => x == '\\' ? '/' : (x == '/' ? '\\' : x)).ToArray());
        videoPlayer.url = videoUrl;
        videoPlayer.Prepare();

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
            Debug.LogError($"Could not fetch executable at path {gamePath}");
        }
    }

    private void PlayVideo(VideoPlayer src)
    {
        videoPlayerImage.texture = videoRenderTexture;
        src.Play();
    }
}
