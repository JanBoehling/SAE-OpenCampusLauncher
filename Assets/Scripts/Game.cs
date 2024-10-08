using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "New Game", menuName = "Game")]
public class Game : ScriptableObject
{
    [field: SerializeField] public string GamePath { get; set; }
    [field: SerializeField] public Sprite Thumbnail { get; set; }
    [field: SerializeField] public VideoClip TrailerVideo { get; set; }

    [field: SerializeField] public string Author { get; set; }
    [field: SerializeField, Range(1, 6)] public int Semester { get; set; } = 1;
    [field: SerializeField, Tooltip("Optional: If not empty, this text is used instead of semester numeration")] public string SemesterAltText { get; set; }
    [field: SerializeField] public string Title { get; set; }
}
