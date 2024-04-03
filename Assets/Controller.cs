using UnityEngine;

public class Controller : MonoBehaviour
{
    private JumpingAnimationComponent _char;
    private bool _isChanged = true;

    private int _startJumps = 5, _endJumps = 5;
    private bool _enableRotate = true;
    private float _timeScale = 1.1f;
    private bool _enableLoop;
    
    private void Start()
    {
        _char = FindAnyObjectByType<JumpingAnimationComponent>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_char.IsPlaying())
            {
                _char.StopTween();
            }
            else
            {
                if (_isChanged)
                {
                    _char.InitializeSequence(_startJumps, _endJumps, _enableRotate, _timeScale);
                    _char.SetLoop(_enableLoop);
                    _isChanged = false;
                }
                _char.RestartTween();
            }
        }
    }

    public void OnEndEditStartJumps(string value)
    {
        if (int.TryParse(value, out var result))
        {
            Debug.Log($"Change StartJumps {result}");
            _startJumps = result;
            _isChanged = true;
        }
    }
    
    public void OnEndEditEndJumps(string value)
    {
        if (int.TryParse(value, out var result))
        {
            Debug.Log($"Change EndJumps {result}");
            _endJumps = result;
            _isChanged = true;
        }
    }
    
    public void OnEndEditEnableRotate(bool value)
    {
        Debug.Log($"Change EnableRotate {value}");
        _enableRotate = value;
        _isChanged = true;
    }
    
    public void OnEndEditTimeScale(string value)
    {
        if (float.TryParse(value, out var result))
        {
            Debug.Log($"Change TimeScale {result}");
            _timeScale = result;
            _isChanged = true;
        }
    }

    public void OnEndEditEnableLoop(bool value)
    {
        Debug.Log($"Set EnableLoop {value}");
        _enableLoop = value;
        _char.SetLoop(_enableLoop);
        _isChanged = true;
    }
}