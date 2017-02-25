using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TweenAlpha : UITweener
{
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1.0f, 1.0f);
    public float Value;
    Color TargetColor;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        this._Type = TweenMode.Color;
        TargetColor = new Color(OriginColor.r, OriginColor.g, OriginColor.b, Value);
    }

    protected override void TweenValue(float t)
    {
        graphic.color = Color.Lerp(OriginColor, TargetColor, curve.Evaluate(t));
    }
}
