using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TweenScale : UITweener
{
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1.0f, 1.0f);
    public Vector3 TargetScale;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        this._Type = TweenMode.Scale;
    }

    protected override void TweenValue(float t)
    {
        graphic.rectTransform.localScale = Vector3.Lerp(OriginScale, TargetScale, curve.Evaluate(t));
    }
}
