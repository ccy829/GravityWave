//----------------------------------------------
//         UITweener for UGUI by Cyan
//            Version: 0.2.0 Beta2
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum TweenMode
{
    Color = 0,
    Position = 1,
    Rotation = 2,
    Scale = 3
};

public enum TweenState
{
    Stop = 0,
    Playing = 1,
    Pause = 2
};

public enum PlayMode
{
    Once = 0,
    Loop = 1,
    PingPong = 2
};

public abstract class UITweener : MonoBehaviour {
    public float duration;
    public PlayMode Mode;
    public delegate void TweenAnimationHandler();
    public TweenAnimationHandler OnFinish;
    public TweenMode Type { get { return _Type; } }
    public TweenState State { get { return _State; } }

    protected Graphic graphic;
    protected float curTime;
    protected float pct;
    protected bool pauseCached = false;
    protected bool reverse = false;
    protected TweenState _State = TweenState.Stop;
    protected TweenMode _Type;
    protected Color OriginColor;
    protected Vector3 OriginPosition;
    protected Vector3 OriginRotation;
    protected Vector3 OriginScale;
    private delegate void TweenUpdateHandler(float delta);
    private TweenUpdateHandler OnUpdate;

    protected virtual void Awake()
    {
        graphic = GetComponent<Graphic>();
        OriginColor = graphic.color;
        OriginPosition = graphic.rectTransform.localPosition;
        OriginRotation = graphic.rectTransform.eulerAngles;
        OriginScale = graphic.rectTransform.localScale;
    }

    void Start() {}

    // Update is called once per frame
    void Update ()
    { 
        if (OnUpdate != null)
        {
            OnUpdate.Invoke(Time.deltaTime);
        }
    }

    void TweenUpdate(float timeElaspe)
    {
        curTime += (curTime + timeElaspe <= duration) ? timeElaspe : duration - curTime;
        pct = curTime / duration;
        TweenValue((!reverse) ? pct : 1.0f - pct);
        if(pct >= 1.0f)
        { 
            if (Mode == PlayMode.Once)
            {
                OnUpdate -= TweenUpdate;
                _State = TweenState.Stop;
                pauseCached = false;
                /*switch (_Type)
                {
                    case TweenMode.Color:
                        OriginColor = graphic.color;
                        break;
                    case TweenMode.Position:
                        OriginPosition = graphic.rectTransform.localPosition;
                        break;
                    case TweenMode.Rotation:
                        OriginRotation = graphic.rectTransform.eulerAngles;
                        break;
                    case TweenMode.Scale:
                        OriginScale = graphic.rectTransform.localScale;
                        break;
                    default:
                        break;
                }*/
                if (OnFinish != null) OnFinish.Invoke();
            }
            else
            {
                if (Mode == PlayMode.PingPong)
                {
                    reverse = !reverse;
                    curTime = 0;
                }
                else Reset();
            }
        }
    }

    protected void Reset()
    {
        curTime = 0;
        switch (_Type)
        {
            case TweenMode.Color:
                graphic.color = OriginColor;
                break;
            case TweenMode.Position:
                graphic.rectTransform.localPosition = OriginPosition;
                break;
            case TweenMode.Rotation:
                graphic.rectTransform.eulerAngles = OriginRotation;
                break;
            case TweenMode.Scale:
                graphic.rectTransform.localScale = OriginScale;
                break;
            default:
                break;
        }
    }

    protected abstract void TweenValue(float t);

    public void Play(bool isReverse)
    {
        if (_State == TweenState.Stop)
        {
            reverse = isReverse;
            curTime = 0;
            _State = TweenState.Playing;
            OnUpdate += TweenUpdate;
        }
    }

    public void Pause(bool toggle)
    { 
        if (toggle != pauseCached) {
            if (toggle)
            {
                OnUpdate -= TweenUpdate;
                _State = TweenState.Pause;
            }
            else
            {
                OnUpdate += TweenUpdate;
                _State = TweenState.Playing;
            }
            pauseCached = toggle;
        }
    }

    public void Stop()
    {
        OnUpdate -= TweenUpdate;
        Reset();
        _State = TweenState.Stop;
        pauseCached = false;
    }
}
