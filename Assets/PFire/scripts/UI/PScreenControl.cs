using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PScreenControl : MonoBehaviour {

    FTweenSca scale;
    ReadText readText;
    void Awake()
    {
        scale = GetComponent<FTweenSca>();
        readText = GetComponentInChildren<ReadText>();
    }
    public void LoadText(string path)
    {
       readText.LoadText(path);
    }
    public bool NextText()
    {
        return readText.NextText();
    }
	public void Show()
    {
        scale.PlayReverse();
    }
    public void Hidden()
    {
        scale.Play();
    }
}
