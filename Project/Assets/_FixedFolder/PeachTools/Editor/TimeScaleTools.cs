using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static partial class Tools {

	[MenuItem ("Tools/Time Scale/ x1")]
	static void TimeScale1 () {
		Time.timeScale = 1f;
	}

	[MenuItem ("Tools/Time Scale/ x5")]
	static void TimeScale5 () {
		Time.timeScale = 5f;
	}

	[MenuItem ("Tools/Time Scale/ x10")]
	static void TimeScale10 () {
		Time.timeScale = 10f;
	}
}