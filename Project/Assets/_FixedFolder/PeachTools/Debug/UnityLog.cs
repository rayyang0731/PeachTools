using System.Collections.Generic;
using UnityEngine;

/// <summary>  
/// A console to display Unity's debug logs in-game.  
/// </summary>  
public class UnityLog : BaseMonoBehaviour {
    struct Log {
        public string message;
        public string stackTrace;
        public LogType type;
    }

    #region Inspector Settings  

    /// <summary>  
    /// Whether to only keep a certain number of logs.  
    ///  
    /// Setting this can be helpful if memory usage is a concern.  
    /// </summary>  
    public bool restrictLogCount = false;

    /// <summary>  
    /// Number of logs to keep before removing old ones.  
    /// </summary>  
    public int maxLogs = 1000;

    #endregion

    readonly List<Log> logs = new List<Log> ();
    public Vector2 scrollPosition;
    bool visible;
    bool collapse;
    bool stackTrace;

    // Visual elements:  

    static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color> { { LogType.Assert, Color.white },
        { LogType.Error, Color.red },
        { LogType.Exception, Color.red },
        { LogType.Log, Color.white },
        { LogType.Warning, Color.yellow },
    };

    const string windowTitle = "Console";
    const string StackTraceFormat = "<<<[ LOG ]<<<<<<<<<<<<\r\n{0}\r\n----- Stack Trace ------:\r\n{1}>>>>>>>>>>>>>>>>>>>>";
    const string LogFormat = "[ LOG ]: {0}";
    const int margin = 5;
    static readonly GUIContent clearLabel = new GUIContent ("Clear", "Clear the contents of the console.");
    static readonly GUIContent closeLabel = new GUIContent ("Close", "Close the console.");
    static readonly GUIContent collapseLabel = new GUIContent ("Collapse", "Hide repeated messages.");
    static readonly GUIContent traceLabel = new GUIContent ("StackTrace", "Open stack trace message.");
    Rect windowRect = new Rect (margin, margin, Screen.width - (margin * 2), Screen.height - (margin * 2));
    Rect buttonRect = new Rect (Screen.width / 2f - 50f, 0f, 100f, 40f);

    void Start () {
        DontDestroyOnLoad (this.gameObject);
    }

    void OnEnable () {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable () {
        Application.logMessageReceived -= HandleLog;
    }

    void OnGUI () {
        if (!visible) {
            if (GUI.Button (buttonRect, windowTitle)) {
                visible = true;
            }
            return;
        }

        windowRect = GUILayout.Window (123456, windowRect, DrawConsoleWindow, windowTitle);
    }

    /// <summary>  
    /// Displays a window that lists the recorded logs.  
    /// </summary>  
    /// <param name="windowID">Window ID.</param>  
    void DrawConsoleWindow (int windowID) {
        if (GUILayout.Button (closeLabel, GUILayout.Height (100)))
            visible = false;

        DrawLogsList ();
        DrawToolbar ();
    }

    /// <summary>  
    /// Displays a scrollable list of logs.  
    /// </summary>  
    void DrawLogsList () {
        GUIStyle font = new GUIStyle (GUI.skin.label);
        font.fontSize = 30;

        scrollPosition = GUILayout.BeginScrollView (scrollPosition);

        // Iterate through the recorded logs.  
        for (var i = 0; i < logs.Count; i++) {
            var log = logs[i];

            // Combine identical messages if collapse option is chosen.  
            if (collapse && i > 0) {
                var previousMessage = logs[i - 1].message;

                if (log.message == previousMessage) {
                    continue;
                }
            }

            GUI.contentColor = logTypeColors[log.type];

            if (stackTrace) {
                GUILayout.Label (string.Format (StackTraceFormat, log.message, log.stackTrace), font);
            } else {
                GUILayout.Label (string.Format (LogFormat, log.message), font);
            }
        }

        GUILayout.EndScrollView ();

        // Ensure GUI colour is reset before drawing other components.  
        GUI.contentColor = Color.white;
    }

    /// <summary>  
    /// Displays options for filtering and changing the logs list.  
    /// </summary>  
    void DrawToolbar () {
        GUILayout.BeginHorizontal ();

        collapse = GUILayout.Toggle (collapse, collapseLabel, GUILayout.ExpandWidth (false));
        GUILayout.Space (100);
        if (GUILayout.Button (clearLabel)) logs.Clear ();
        GUILayout.Space (100);
        stackTrace = GUILayout.Toggle (stackTrace, traceLabel, GUILayout.ExpandWidth (false));

        GUILayout.EndHorizontal ();
    }

    /// <summary>  
    /// Records a log from the log callback.  
    /// </summary>  
    /// <param name="message">Message.</param>  
    /// <param name="stackTrace">Trace of where the message came from.</param>  
    /// <param name="type">Type of message (error, exception, warning, assert).</param>  
    void HandleLog (string message, string stackTrace, LogType type) {
        logs.Add (new Log {
            message = message,
                stackTrace = stackTrace,
                type = type,
        });

        TrimExcessLogs ();
    }

    /// <summary>  
    /// Removes old logs that exceed the maximum number allowed.  
    /// </summary>  
    void TrimExcessLogs () {
        if (!restrictLogCount) {
            return;
        }

        var amountToRemove = Mathf.Max (logs.Count - maxLogs, 0);

        if (amountToRemove == 0) {
            return;
        }

        logs.RemoveRange (0, amountToRemove);
    }
}