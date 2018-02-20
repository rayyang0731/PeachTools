using UnityEngine;

public abstract class BaseMonoBehaviour : MonoBehaviour {
    private void Awake () {
        UpdateManager.Instance.Add (this);
        _Awake ();
    }
    protected virtual void _Awake () { }

    private void OnDestroy () {
        UpdateManager.Instance.Remove (this);
        _OnDestroy ();
    }

    protected virtual void _OnDestroy () { }
}