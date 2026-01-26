using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    private VisualElement _bottomContainer;
    private Button _openButton;
    private Button _closeButton;

    void Start()
    {
        var doc = GetComponent<UIDocument>();
        if (doc == null)
        {
            Debug.LogError("UIController: No UIDocument found on this GameObject.", this);
            return;
        }

        var root = doc.rootVisualElement;
        if (root == null)
        {
            Debug.LogError("UIController: rootVisualElement is null. Check the UIDocument has a valid UXML assigned.", this);
            return;
        }

        _bottomContainer = root.Q<VisualElement>("ContainerBottom");
        _openButton = root.Q<Button>("ButtonOpen");
        _closeButton = root.Q<Button>("ButtonClose");

        if (_bottomContainer == null) Debug.LogError("UIController: Could not find VisualElement named 'ContainerBottom'.", this);
        if (_openButton == null) Debug.LogError("UIController: Could not find Button named 'ButtonOpen'.", this);
        if (_closeButton == null) Debug.LogError("UIController: Could not find Button named 'ButtonClose'.", this);

        if (_bottomContainer == null || _openButton == null || _closeButton == null)
            return;

        _bottomContainer.style.display = DisplayStyle.None;

        _openButton.clicked += OnOpenButtonClicked;
        _closeButton.clicked += OnCloseButtonClicked;
    }

    private void OnOpenButtonClicked()
    {
        _bottomContainer.style.display = DisplayStyle.Flex;
    }

    private void OnCloseButtonClicked()
    {
        _bottomContainer.style.display = DisplayStyle.None;
    }
}

