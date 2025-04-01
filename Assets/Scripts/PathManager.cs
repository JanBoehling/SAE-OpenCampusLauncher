using System.IO;
using UnityEngine;

namespace ProgramUtils
{
    public static class PathManager
    {
        public readonly static string pathFilePath = Directory.GetCurrentDirectory() + System.IO.Path.DirectorySeparatorChar + "Paths.json";

        public static Path Paths { get; private set; }
        public static Path PathFile => Paths;

        public static Path LoadPathFile()
        {
            if (!File.Exists(pathFilePath))
            {
                GeneratePathFile();
                var errorText = GameObject.FindGameObjectWithTag("ErrorCanvas").GetComponentInChildren<TMPro.TMP_Text>();

                errorText.text = $"FEHLER: Pfad-Datei fehlt. Öffne Paths.json und fülle die Felder aus. Datei befindet sich im Pfad {pathFilePath}";
                
                return Paths;
            }
            else Object.Destroy(GameObject.FindGameObjectWithTag("ErrorCanvas"));

            using var reader = new StreamReader(pathFilePath);
            string json = reader.ReadToEnd();

            Paths = JsonUtility.FromJson<Path>(json);

            return Paths;
        }

        public static void GeneratePathFile()
        {
            string json = JsonUtility.ToJson(new Path(), true);
            File.Create(pathFilePath).Close();
            using var stream = new StreamWriter(pathFilePath);
            stream.Write(json);
            stream.Close();
        }
    }

    [System.Serializable]
    public struct Path
    {
        public string GameFileBasePath;
        public string DetailSelectionBasePath;

        public Path(string gameFileBasePath = null, string detailSelectionBasePath = null)
        {
            GameFileBasePath = gameFileBasePath is null ? Directory.GetCurrentDirectory() + System.IO.Path.DirectorySeparatorChar + "DemoGames" + System.IO.Path.DirectorySeparatorChar : gameFileBasePath;
            DetailSelectionBasePath = detailSelectionBasePath is null ? GameFileBasePath + "_Selection" + System.IO.Path.DirectorySeparatorChar : detailSelectionBasePath;
        }
    }
}
