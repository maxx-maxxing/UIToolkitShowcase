using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    private VisualElement _bottomContainer;
    private Button _openButton;
    private Button _closeButton;
    private VisualElement _bottomSheet;
    private VisualElement _scrim;
    private VisualElement _boy;
    private VisualElement _robot;
    private Label _message;

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
        _boy = root.Q<VisualElement>("Boy");
        _robot = root.Q<VisualElement>("Robot");
        _message = root.Q<Label>("Message");

        if (_bottomContainer == null) Debug.LogError("UIController: Could not find VisualElement named 'ContainerBottom'.", this);
        if (_openButton == null) Debug.LogError("UIController: Could not find Button named 'ButtonOpen'.", this);
        if (_closeButton == null) Debug.LogError("UIController: Could not find Button named 'ButtonClose'.", this);
        if (_bottomSheet == null) Debug.LogError("UIController: Could not find Sheet named 'BottomSheet'.", this);
        if (_scrim == null) Debug.LogError("UIController: Could not find Sheet named 'Scrim'.", this);
        if (_boy == null) Debug.LogError("UIController: Could not find Sheet named 'Boy'.", this);
        if (_robot == null) Debug.LogError("UIController: Could not find Robot named 'Robot'.", this);
        if (_message == null) Debug.LogError("UIController: Could not find Message named 'Message'.", this);
        if (_bottomContainer == null || _openButton == null || _closeButton == null || _bottomSheet == null || _scrim == null || _boy == null || _robot == null || _message == null)
            return;

        _bottomContainer.style.display = DisplayStyle.None;
        
        Invoke(nameof(AnimateBoy), .1f);
        
        _bottomSheet.RegisterCallback<TransitionEndEvent>(OnBottomSheetDown);

        _openButton.clicked += OnOpenButtonClicked;
        _closeButton.clicked += OnCloseButtonClicked;
        
        


    }

    

    private void AnimateBoy()
    {
        _boy.RemoveFromClassList("image--boy--inair");
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

        AnimateRobot();


    }

    private void AnimateRobot()
    {
        _robot.ToggleInClassList("image--robot--up");
        _robot.RegisterCallback<TransitionEndEvent>(
            evt => _robot.ToggleInClassList("image--robot--up")
        );
        
        _message.text = string.Empty;
        string m = "Sed in rebus apertissimis nimium longi sumus.";
        DOTween.To(() => _message.text, x => _message.text = x, m, 3f) .SetEase(Ease.Linear);

    }

    private void OnCloseButtonClicked()
    {
        // Start fade-out
        _scrim.RemoveFromClassList("scrim--fadein");
        _bottomSheet.RemoveFromClassList("bottomsheet--up");

    }

    private void OnBottomSheetDown(TransitionEndEvent evt)
    {
        if (!_bottomSheet.ClassListContains("bottomsheet--up"))
        {
            _bottomContainer.style.display = DisplayStyle.None;
        }
    }
    
   




}

