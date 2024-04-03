using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class JumpingAnimationComponent : MonoBehaviour
{
    private Image _image;
    private Sequence _seq;
    private bool _requestLoopChange;
    private bool _enableLoop;

    public void InitializeSequence(int startJumps, int endJumps, bool enableRotate, float timeScale)
    {
        DOTween.Kill(gameObject, true);
        var seq = DOTween.Sequence().SetTarget(gameObject);
        var seq2 = DOTween.Sequence().SetTarget(gameObject);

        var jumpLoop = DOTween.Sequence().SetTarget(gameObject);
        for (var i = 0; i < startJumps; i++)
        {
            jumpLoop.Append(_image.transform.DOScaleY(0.85f, 0.07f));
            jumpLoop.Join(_image.transform.DOScaleX(1.15f, 0.07f));
            
            jumpLoop.Append(_image.transform.DOLocalJump(_image.transform.localPosition, 15, 1, 0.18f));
            jumpLoop.Join(_image.transform.DOScaleY(1f, 0.07f));
            jumpLoop.Join(_image.transform.DOScaleX(1f, 0.07f));
        }
        
        var jumpLoop2 = DOTween.Sequence().SetTarget(gameObject);
        for (var i = 0; i < endJumps; i++)
        {
            jumpLoop2.Append(_image.transform.DOScaleY(0.9f, 0.07f));
            jumpLoop2.Join(_image.transform.DOScaleX(1.1f, 0.07f));
            
            jumpLoop2.Append(_image.transform.DOLocalJump(_image.transform.localPosition, 15, 1, 0.18f));
            jumpLoop2.Join(_image.transform.DOScaleY(1f, 0.07f));
            jumpLoop2.Join(_image.transform.DOScaleX(1f, 0.07f));
        }
        
        var connected = DOTween.Sequence().SetTarget(gameObject);
        
        seq2.Prepend(_image.transform.DOScaleY(0.8f, 0.1f));
        seq2.Join(_image.transform.DOScaleX(1.2f, 0.1f));
        seq2.Append(_image.transform.DOScaleY(1.2f, 0.2f));
        seq2.Join(_image.transform.DOScaleX(0.8f, 0.2f));
        seq2.Append(_image.transform.DOScaleY(1f, 0.2f));
        seq2.Join(_image.transform.DOScaleX(1f, 0.2f));
        
        seq.Insert(0, _image.transform.DOBlendableLocalRotateBy(new Vector3(0, 0, -360), 0.8f, RotateMode.LocalAxisAdd).SetEase(Ease.InOutBack, 1.7f));
        seq.Insert(0.08f, _image.transform.DOLocalMoveY(_image.transform.localPosition.y + 120, 0.55f).SetEase(Ease.OutCirc));
        seq.Insert(0.67f, _image.transform.DOLocalMoveY(_image.transform.localPosition.y, 0.2f).SetEase(Ease.InSine));

        if (startJumps > 0)
        {
            connected.Append(jumpLoop);
        }

        if (enableRotate)
        {
            connected.Append(seq2);
            connected.Join(seq);
        }

        if (endJumps > 0)
        {
            connected.Append(jumpLoop2);
        }

        if (connected.Duration() == 0)
        {
            _seq = null;
            return;
        }
        
        connected.timeScale = timeScale;
        _seq = connected.SetAutoKill(false);
        _seq.onStepComplete += OnStepComplete;
    }
    
    private void Start()
    {
        _image = GetComponent<Image>();
    }

    public bool IsPlaying()
    {
        return _seq != null && _seq.IsPlaying();
    }

    public void SetLoop(bool enable)
    {
        if (enable)
        {
            _seq?.SetLoops(-1);
            _enableLoop = true;
            _requestLoopChange = true;
        }
        else
        {
            _seq?.SetLoops(0);
            _enableLoop = false;
            _requestLoopChange = true;
        }
    }

    private void OnStepComplete()
    {
        if (_requestLoopChange)
        {
            if (_enableLoop)
            {
                _seq?.Restart();
            }
            else
            {
                StopTween();
            }
            
            _requestLoopChange = false;
        }
    }

    public void StopTween()
    {
        _seq.Pause();
        _seq.Goto(0);
    }
    
    public void RestartTween()
    {
        _seq?.Restart(); 
    }
}