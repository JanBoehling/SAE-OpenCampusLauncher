using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadGameSelection : MonoBehaviour
{
    [SerializeField, Tooltip("Debug")] private bool _loadImages = true;
    [SerializeField] private GameObject gameCardPrefab;
    [SerializeField] private Transform[] gameCardAnchorRows;
    
    private GameDetailsLoader gameDetailsLoader;


    //private static readonly string gameBasePath = ProgramUtils.PathManager.Paths.GameFileBasePath;
    //private static readonly string gameDetailsPath = ProgramUtils.PathManager.Paths.DetailSelectionBasePath;

    //private static readonly string gameBasePath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "DemoGames" + Path.DirectorySeparatorChar;
    //private static readonly string gameDetailsPath = gameBasePath + "_Selection" + Path.DirectorySeparatorChar;

    private Game[] games;

    private void Awake()
    {
        ProgramUtils.PathManager.LoadPathFile();

        gameDetailsLoader = FindFirstObjectByType<GameDetailsLoader>();
    }

    private void Start()
    {
        games = LoadGamesFromFolder(ProgramUtils.PathManager.Paths.DetailSelectionBasePath);
        GenerateGameCards(games);

        PageMoveController.SetPageAmount(Mathf.RoundToInt(Mathf.Ceil(games.Length / 8f)));
    }

    [ContextMenu("Load Games From Folder")]
    private static Game[] LoadGamesFromFolder(string path)
    {
        //var rawGameDetails = Resources.LoadAll<TextAsset>("Games");
        var rawGameDetailsPath = Directory.GetFiles(path);
        int gameAmount = rawGameDetailsPath.Length;

        var gameDetails = new Game[gameAmount];

        for (int i = 0; i < gameAmount; i++)
        {
            string jsonContent = File.ReadAllText(rawGameDetailsPath[i]);
            gameDetails[i] = (Game)JsonUtility.FromJson(jsonContent, typeof(Game));
        }

        return gameDetails;
    }

    private void GenerateGameCards(Game[] gameDetails)
    {
        if (!gameCardPrefab)
        {
            Debug.LogError("There is no game card prefab selected.");
            return;
        }

        int rowAmount = gameCardAnchorRows.Length;

        int selectedRow = 0;

        for (int i = 0; i < gameDetails.Length; i++)
        {
            var game = gameDetails[i];

            var gameCardBG = Instantiate(gameCardPrefab, gameCardAnchorRows[selectedRow++]);
            var gameCard = Instantiate(gameCardPrefab, gameCardBG.transform);

            gameCardBG.GetComponent<Image>().sprite = null;
            gameCard.transform.localScale *= .9f;

            if (_loadImages) gameCard.GetComponent<Image>().sprite = LoadSpriteFromFile(gameDetails[i].ThumbnailPath);

            var cardButton = gameCard.AddComponent<Button>();
            cardButton.transition = Selectable.Transition.None;
            cardButton.onClick.AddListener(() => gameDetailsLoader.LoadDetails(game));

            if (selectedRow == rowAmount) selectedRow = 0;
        }
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
