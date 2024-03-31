using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class JumpingAnimationComponent : MonoBehaviour
{
    private Image _image;
    private Sequence _seq;
    
    private void Start()
    {
        _image = GetComponent<Image>();
        
        _seq = JumpingSequence().SetAutoKill(false).Play();
        _seq.DOTimeScale(1.1f, 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Break();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _seq.Restart();
        }
    }

    private Sequence JumpingSequence()
    {
        var seq = DOTween.Sequence();
        var seq2 = DOTween.Sequence();

        var jumpLoop = DOTween.Sequence();
        for (var i = 0; i < 5; i++)
        {
            jumpLoop.Append(_image.transform.DOScaleY(0.85f, 0.07f));
            jumpLoop.Join(_image.transform.DOScaleX(1.15f, 0.07f));
            
            jumpLoop.Append(_image.transform.DOLocalJump(_image.transform.localPosition, 15, 1, 0.18f));
            jumpLoop.Join(_image.transform.DOScaleY(1f, 0.07f));
            jumpLoop.Join(_image.transform.DOScaleX(1f, 0.07f));
        }
        
        var jumpLoop2 = DOTween.Sequence();
        for (var i = 0; i < 5; i++)
        {
            jumpLoop2.Append(_image.transform.DOScaleY(0.9f, 0.07f));
            jumpLoop2.Join(_image.transform.DOScaleX(1.1f, 0.07f));
            
            jumpLoop2.Append(_image.transform.DOLocalJump(_image.transform.localPosition, 15, 1, 0.18f));
            jumpLoop2.Join(_image.transform.DOScaleY(1f, 0.07f));
            jumpLoop2.Join(_image.transform.DOScaleX(1f, 0.07f));
        }
        

        var connected = DOTween.Sequence();
        
        seq2.Prepend(_image.transform.DOScaleY(0.8f, 0.1f));
        seq2.Join(_image.transform.DOScaleX(1.2f, 0.1f));
        seq2.Append(_image.transform.DOScaleY(1.2f, 0.2f));
        seq2.Join(_image.transform.DOScaleX(0.8f, 0.2f));
        seq2.Append(_image.transform.DOScaleY(1f, 0.2f));
        seq2.Join(_image.transform.DOScaleX(1f, 0.2f));
        
        seq.Insert(0, _image.transform.DOBlendableLocalRotateBy(new Vector3(0, 0, -360), 0.8f, RotateMode.LocalAxisAdd).SetEase(Ease.InOutBack, 1.7f));
        seq.Insert(0.08f, _image.transform.DOLocalMoveY(_image.transform.localPosition.y + 120, 0.55f).SetEase(Ease.OutCirc));
        seq.Insert(0.67f, _image.transform.DOLocalMoveY(_image.transform.localPosition.y, 0.2f).SetEase(Ease.InSine));

        connected.Append(jumpLoop);
        
        connected.Append(seq2);
        connected.Join(seq);
        
        connected.Append(jumpLoop2);
        
        return connected;
    }
}