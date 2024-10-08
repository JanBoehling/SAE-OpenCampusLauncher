using UnityEngine;
using UnityEngine.UI;

public class GameSelector : MonoBehaviour
{
    [SerializeField] private Game game;
    public Game Game => game;

    private void Start()
    {
        if (!game.Thumbnail) return;
        GetComponent<Image>().sprite = game.Thumbnail;
    }
}
