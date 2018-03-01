using System.Collections;
using UnityEngine;

public class GameFPS : BaseMonoBehaviour, IUpdate {
    public float updateInterval = 0.5f;

    private float lastInterval;
    private float frames = 0;
    private float fps;

    private Color color;

    private static GUIStyle _normalLabelStyle = null;

    public static GUIStyle NormalLabelStyle {
        get {
            if (_normalLabelStyle == null) {
                _normalLabelStyle = new GUIStyle (GUI.skin.label);
                _normalLabelStyle.fontSize = 40;
                _normalLabelStyle.fontStyle = FontStyle.Bold;
            }
            return _normalLabelStyle;
        }
    }

    private void OnGUI () {
        GUI.color = color;
        GUI.Label (
            new Rect (5, 5, 300, 50),
            string.Format ("FPS: {0}", fps.ToString ("f2")), NormalLabelStyle);
    }

    private void Start () {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
    }

    public void _Update () {
        ++frames;
        var timeNow = Time.realtimeSinceStartup;
        if (timeNow > lastInterval + updateInterval) {
            fps = frames / (timeNow - lastInterval);
            frames = 0;
            lastInterval = timeNow;
        }
        if (fps < 25)
            color = Color.yellow;
        else if (fps < 15)
            color = Color.red;
        else
            color = Color.green;
    }
}