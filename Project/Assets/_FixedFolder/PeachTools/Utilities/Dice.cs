using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 摇奖
/// </summary>
public static class Dice {
	/// <summary>
	/// 获得摇奖结果
	/// </summary>
	/// <param name="store">参与摇奖的对象( k - 摇奖对象标记, v - 摇奖对象中奖概率)</param>
	/// <returns>中奖对象标记</returns>
	public static object GetResult (ListKV<object, int> store) {
		int jackpot = 1;
		for (int i = 0; i < store.Count; i++)
			jackpot += store[i].Val;

		int target = Random.Range (0, jackpot);

		for (int i = 0; i < store.Count; i++) {
			int _minVal = i == 0 ? 0 : store[i - 1].Val;
			int _maxVal = 0;
			for (int j = 0; j <= i; j++) {
				_maxVal += store[j].Val;
			}
			if (target >= _minVal && target < _maxVal) {
				return store[i].Key;
			}
		}

		throw new System.Exception ("获取摇奖结果失败,请检查");
	}

	/// <summary>
	/// 获得摇奖结果
	/// </summary>
	/// <param name="store">参与摇奖的对象(的概率)</param>
	/// <returns>中奖对象的索引</returns>
	public static int GetResult (int[] store) {
		ListKV<object, int> _store = new ListKV<object, int> ();
		for (int i = 0; i < store.Length; i++)
			_store.Add (i, store[i]);

		return (int) GetResult (_store);
	}
}