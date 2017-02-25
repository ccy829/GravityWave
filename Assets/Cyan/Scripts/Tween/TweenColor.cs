using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TweenColor : UITweener
{
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1.0f, 1.0f);
    public Color TargetColor;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        this._Type = TweenMode.Color;
    }

    protected override void TweenValue(float t)
    {
        graphic.color = Color.Lerp(OriginColor, TargetColor, curve.Evaluate(t));
    }
}
