using System.Diagnostics;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public void StartExe(string absolutPath)
    {
        var process = new ProcessStartInfo(absolutPath)
        {
            WindowStyle = ProcessWindowStyle.Maximized,
        };

        Process.Start(process);
    }
}
