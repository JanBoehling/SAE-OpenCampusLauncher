using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameSelector : MonoBehaviour
{
    [SerializeField] private Game game;

    private void Start()
    {
        GetComponent<Image>().sprite = game.Thumbnail;
    }
}
