using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class PageMoveController : MonoSingleton<PageMoveController>
{
    [SerializeField] private RectTransform _gameSelectionContainer;
    [SerializeField] private float _animationDuration = 1f;
    [SerializeField] private AnimationCurve _pageMoveAnimationCurve;

    private int _pageAmount = 0;

    private Button[] _buttons;

    public int CurrentPage { get; private set; }

    private Coroutine _pageMoveAnimation;

    private Func<bool> pageLeftMovePredicate;
    private Func<bool> pageRightMovePredicate;

    protected override void Awake()
    {
        base.Awake();
        _buttons = GetComponentsInChildren<Button>();

        pageLeftMovePredicate = () => CurrentPage > 0;
        pageRightMovePredicate = () => CurrentPage < _pageAmount - 1;
    }

    private void Update()
    {
        if ((Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A)) && pageLeftMovePredicate.Invoke())
        {
            MovePage(Direction.Left);
        }

        else if ((Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D)) && pageRightMovePredicate.Invoke())
        {
            MovePage(Direction.Right);
        }
    }

    private void MovePage(Direction direction)
    {
        if (_pageMoveAnimation != null) return;

        CurrentPage -= (int)direction;
        _pageMoveAnimation = StartCoroutine(MovePageCO((int)direction));

        UpdateInteractivity();
    }

    public void MovePage(SerializableEnumComponent direction)
    {
        MovePage(direction.Value);
    }

    private void UpdateInteractivity()
    {
        _buttons[0].interactable = pageLeftMovePredicate.Invoke();
        _buttons[1].interactable = pageRightMovePredicate.Invoke();
    }

    public void SetPageAmount(int amount)
    {
        _pageAmount = amount;
        UpdateInteractivity();
    }

    private IEnumerator MovePageCO(int direction)
    {
        var priorPos = _gameSelectionContainer.localPosition;
        var targetPos = priorPos + _gameSelectionContainer.rect.width * direction * Vector3.right;

        float elapsedTime = 0f;

        while (elapsedTime < _animationDuration)
        {
            float t = elapsedTime / _animationDuration;

            var newPos = Vector3.Lerp(priorPos, targetPos, _pageMoveAnimationCurve.Evaluate(t));
            _gameSelectionContainer.localPosition = newPos;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _gameSelectionContainer.localPosition = targetPos;

        _pageMoveAnimation = null;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PageMoveController))]
public class PageMoveControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox($"Current Page: {PageMoveController.Instance.CurrentPage}", MessageType.None, true);
    }
    public override bool RequiresConstantRepaint() => true;
}
#endif
