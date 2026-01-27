using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    private VisualElement _bottomContainer;
    private Button _openButton;
    private Button _closeButton;
    private VisualElement _bottomSheet;
    private VisualElement _scrim;

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
        _bottomSheet = root.Q<VisualElement>("BottomSheet");
        _scrim = root.Q<VisualElement>("Scrim");

        if (_bottomContainer == null) Debug.LogError("UIController: Could not find VisualElement named 'ContainerBottom'.", this);
        if (_openButton == null) Debug.LogError("UIController: Could not find Button named 'ButtonOpen'.", this);
        if (_closeButton == null) Debug.LogError("UIController: Could not find Button named 'ButtonClose'.", this);
        if (_bottomSheet == null) Debug.LogError("UIController: Could not find Sheet named 'BottomSheet'.", this);
        if (_scrim == null) Debug.LogError("UIController: Could not find Sheet named 'Scrim'.", this);

        if (_bottomContainer == null || _openButton == null || _closeButton == null || _bottomSheet == null || _scrim == null)
            return;

        _bottomContainer.style.display = DisplayStyle.None;

        _openButton.clicked += OnOpenButtonClicked;
        _closeButton.clicked += OnCloseButtonClicked;
        
        
    }

    private void OnOpenButtonClicked()
    {
        _bottomContainer.style.display = DisplayStyle.Flex;

        // reset start states
        _scrim.RemoveFromClassList("scrim--fadein");
        _bottomSheet.RemoveFromClassList("bottomsheet--up");

        // next frame: animate
        _bottomSheet.schedule.Execute(() =>
        {
            _scrim.AddToClassList("scrim--fadein");
            _bottomSheet.AddToClassList("bottomsheet--up");
        });
        

    }



    private void OnCloseButtonClicked()
    {
        // Start fade-out
        _scrim.RemoveFromClassList("scrim--fadein");
        _bottomSheet.RemoveFromClassList("bottomsheet--up");

        // After animation finishes, hide container
        _bottomSheet.schedule.Execute(() =>
        {
            _bottomContainer.style.display = DisplayStyle.None;
        }).StartingIn(1000); // match your 1s transition

    }
    
   




}

