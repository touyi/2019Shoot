using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * scale value change
 */
public class FTweenSca : MonoBehaviour {

    public Vector3 from;
    public Vector3 to;
    public float time;
    public float delay;
    public iTween.EaseType easetype;
    public iTween.LoopType looptype;
    public delegate void Onevent();
    public Onevent _start;
    public Onevent _end;
    public bool autoPlay = true;
    void Start()
    {
        if(autoPlay)
        {
            Play();
        }
        
        
    }
    public void Play()
    {
        Hashtable args = new Hashtable();
        transform.localScale = from;
        args.Add("scale", to);
        
        args.Add("time", time);
        args.Add("delay", delay);
        args.Add("easeType", easetype);
        args.Add("loopType", looptype);
        args.Add("onstart", "PlayStart");
        args.Add("oncomplete", "PlayEnd");
        iTween.ScaleTo(this.gameObject, args);
    }
    public void PlayReverse()
    {
        Hashtable args = new Hashtable();
        transform.localScale = to;
        args.Add("scale", from);
        
        args.Add("time", time);
        args.Add("delay", delay);
        args.Add("easeType", easetype);
        args.Add("loopType", looptype);
        args.Add("onstart", "PlayStart");
        args.Add("oncomplete", "PlayEnd");

        iTween.ScaleTo(this.gameObject, args);
    }
    void PlayStart()
    {
        if(_start!=null)
        {
            _start();
        }
    }
    void PlayEnd()
    {
        if(_end!=null)
        {
            _end();
        }
    }
}
