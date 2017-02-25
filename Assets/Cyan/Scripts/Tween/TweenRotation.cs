using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TweenRotation : UITweener
{
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1.0f, 1.0f);
    public Vector3 TargetRotation;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        this._Type = TweenMode.Rotation;
    }

    protected override void TweenValue(float t)
    {
        graphic.rectTransform.eulerAngles = Vector3.Lerp(OriginRotation, TargetRotation, curve.Evaluate(t));
    }
}
