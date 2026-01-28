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

    private const float BoyAnimationDelay = 0.1f;
    private const float TypeDuration = 3.0f;
    private const string MessageText = "Sed in rebus apertissimis nimium longi sumus.";

    void Start()
    {
        if (!InitializeUI() || !ValidateUI())
        {
            enabled = false;
            return;
        }
        /* ^^ 1) Do I have a UIDocument and a root, and can I query my elements?
              2) Did those queries actually find everything I need (if not, log what’s missing) */

        _bottomContainer.style.display = DisplayStyle.None;
        // ^^ Hide container for the pop-up slider
        
        Invoke(nameof(AnimateBoy), BoyAnimationDelay);
        /* ^^ After x time, call AnimateBoy(), which removes
         "image--boy--inair" from its respective class list */
        
        _bottomSheet.RegisterCallback<TransitionEndEvent>(OnBottomSheetDown);
        // ^^ When bottom sheet finishes animating, call OnBottomSheetDown()

        _openButton.clicked += OnOpenButtonClicked;
        _closeButton.clicked += OnCloseButtonClicked;
    }

    private bool InitializeUI()
    {
        var doc = GetComponent<UIDocument>();
        if (doc == null)
        {
            Debug.LogError("UIController: No UIDocument found on this GameObject.", this);
            return false;
        }

        var root = doc.rootVisualElement;
        if (root == null)
        {
            Debug.LogError("UIController: rootVisualElement is null. Check the UIDocument has a valid UXML assigned.", this);
            return false;
        }

        _bottomContainer = root.Q<VisualElement>("ContainerBottom");
        _openButton = root.Q<Button>("ButtonOpen");
        _closeButton = root.Q<Button>("ButtonClose");
        _bottomSheet = root.Q<VisualElement>("BottomSheet");
        _scrim = root.Q<VisualElement>("Scrim");
        _boy = root.Q<VisualElement>("Boy");
        _robot = root.Q<VisualElement>("Robot");
        _message = root.Q<Label>("Message");

        return true;
    }
    // ^^ Do I have a UIDocument and a root, and can I query my elements?
    
    private bool ValidateUI()
    {
        if (_bottomContainer == null) Debug.LogError("UIController: Could not find VisualElement named 'ContainerBottom'.", this);
        if (_openButton == null) Debug.LogError("UIController: Could not find Button named 'ButtonOpen'.", this);
        if (_closeButton == null) Debug.LogError("UIController: Could not find Button named 'ButtonClose'.", this);
        if (_bottomSheet == null) Debug.LogError("UIController: Could not find VisualElement named 'BottomSheet'.", this);
        if (_scrim == null) Debug.LogError("UIController: Could not find VisualElement named 'Scrim'.", this);
        if (_boy == null) Debug.LogError("UIController: Could not find VisualElement named 'Boy'.", this);
        if (_robot == null) Debug.LogError("UIController: Could not find VisualElement named 'Robot'.", this);
        if (_message == null) Debug.LogError("UIController: Could not find Label named 'Message'.", this);

        return _bottomContainer != null &&
               _openButton != null &&
               _closeButton != null &&
               _bottomSheet != null &&
               _scrim != null &&
               _boy != null &&
               _robot != null &&
               _message != null;
    }
    // ^^ Did those queries actually find everything I need (and if not, log exactly what’s missing)

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
        /* ^^ Just in case bottomsheet--up and/or scrim--fadein is in the class list
         for those elements, remove them before running execute to add them */

        // next frame: animate
        _bottomSheet.schedule.Execute(() =>
        {
            _scrim.AddToClassList("scrim--fadein");
            _bottomSheet.AddToClassList("bottomsheet--up");
        });
        // ^^ Run this code the following update frame

        AnimateRobot();


    }

    private void AnimateRobot()
    {
        _robot.ToggleInClassList("image--robot--up");
        _robot.RegisterCallback<TransitionEndEvent>(OnRobotTransitionEnd);
        // ^^ When animation finishes, call passed fxn

        _message.text = string.Empty;
        // ^^ Set _message to Empty
        
        DOTween.To(() => _message.text, x => _message.text = x, MessageText, TypeDuration).SetEase(Ease.Linear);
        // ^^ Every frame, slowly change this variable until it equals that value.
    }
    
    private void OnRobotTransitionEnd(TransitionEndEvent evt)
    {
        _robot.ToggleInClassList("image--robot--up");
        _robot.UnregisterCallback<TransitionEndEvent>(OnRobotTransitionEnd);
        // ^^ After animation finishes, turn off listener
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
    
    private void OnDestroy()
    {
        
        if (_bottomSheet != null)
            _bottomSheet.UnregisterCallback<TransitionEndEvent>(OnBottomSheetDown);

        if (_robot != null)
            _robot.UnregisterCallback<TransitionEndEvent>(OnRobotTransitionEnd);

        // Unsubscribe button delegates
        if (_openButton != null)
            _openButton.clicked -= OnOpenButtonClicked;

        if (_closeButton != null)
            _closeButton.clicked -= OnCloseButtonClicked;
    }
    // ^^ Before this object is destroyed, remove all event subscriptions to avoid bugs






}

