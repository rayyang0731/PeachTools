using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILabel : Text, IUIRefresh {

    private string _colorKey;

    private string _languageKey;

    public string ColorKey {
        get {
            return _colorKey;
        }
        set {
            if (_colorKey != value)
                _colorKey = value;
        }
    }

    protected override void Start () {
        base.Start ();
        OnRefresh ();
    }

    public string LanaguageKey {
        get {
            return _languageKey;
        }
        set {
            _languageKey = value;
        }
    }

    public void UpdateColorKey () {
        if (!string.IsNullOrEmpty (_colorKey)) {
            this.color = Config.GetColor (_colorKey);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty (this);
#endif
        }
    }

    public void UpdateLanguageKey () {
        if (!string.IsNullOrEmpty (_languageKey)) {
            this.text = Language.GetValue (_languageKey);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty (this);
#endif
        }
    }

    public void OnRefresh () {
        UpdateLanguageKey ();
        UpdateColorKey ();
    }
}