using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TweenPosition : UITweener
{
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1.0f, 1.0f);
    public Vector3 TargetPosition;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        this._Type = TweenMode.Position;
    }

    protected override void TweenValue(float t)
    {
        graphic.rectTransform.localPosition = Vector3.Lerp(OriginPosition, TargetPosition, curve.Evaluate(t));
    }
}