using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

/* TODO: 
 * add page move anim
 * toggle interactivity on other button
 * fancy up UI
 * add button sprite (use button sprite from either lost in time or kleptomanicat)
 * add video is loading sprite
 */

public class PageMover : MonoBehaviour
{
    private static int pageAmount = 0;

    private static int currentPage = 0;

    private Button button;

    public static int CurrentPage => currentPage;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void MovePageLeft()
    {
        
        //do anim
        button.interactable = --currentPage != 0;
        return;
    }

    public void MovePageRight()
    {
        if (currentPage == pageAmount)
        {
            button.interactable = false;
            return;
        }
        //do anim
        button.interactable = ++currentPage != pageAmount - 1;
        return;
    }

    public static void SetPageAmount(int amount) => pageAmount = amount;
}

#if UNITY_EDITOR
[CustomEditor(typeof(PageMover))]
public class PageMoverEditor : Editor
{
    public override void OnInspectorGUI() => EditorGUILayout.LabelField($"Current Page: {PageMover.CurrentPage}");
}
#endif
