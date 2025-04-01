using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    protected override void Awake()
    {
        base.Awake();
        _buttons = GetComponentsInChildren<Button>();
    }

    public void MovePage(SerializableEnumComponent direction)
    {
        if (_pageMoveAnimation != null) return;

        CurrentPage -= (int)direction.Value;
        _pageMoveAnimation = StartCoroutine(MovePageCO((int)direction.Value));

        UpdateInteractivity();
    }

    private void UpdateInteractivity()
    {
        _buttons[0].interactable = CurrentPage > 0;
        _buttons[1].interactable = CurrentPage < _pageAmount - 1;
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
public class PageMoverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField($"Current Page: {PageMoveController.Instance.CurrentPage}");
    }
    public override bool RequiresConstantRepaint() => true;
}
#endif
