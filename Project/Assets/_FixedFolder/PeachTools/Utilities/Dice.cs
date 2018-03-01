using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 计算随机数公式
/// </summary>
/// <param name="min">最小值</param>
/// <param name="max">最大值</param>
/// <param name="seed">随机种子</param>
/// <returns>所得随机数</returns>
public delegate int CustomRandomFormula (int min, int max, int seed);
/// <summary>
/// 摇奖
/// </summary>
public static class Dice {
	/// <summary>
	/// 获得中奖人
	/// </summary>
	/// <param name="store">参与摇奖的对象( k - 摇奖对象标记, v - 摇奖对象中奖概率)</param>
	/// <param name="custom">自定义随机公式</param>
	/// <param name="seed">随机种子</param>
	/// <returns>中奖对象标记</returns>
	public static T GetWinner<T> (ListKV<T, int> store, CustomRandomFormula custom = null, int seed = -1) {
		int jackpot = 1;
		for (int i = 0; i < store.Count; i++)
			jackpot += store[i].Val;

		int target = GetRandom (0, jackpot, custom, seed);
		Debug.LogFormat ("[Dice] random : {0}", target);

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
	/// 获得中奖人
	/// </summary>
	/// <param name="store">参与摇奖的对象(的概率)</param>
	/// <param name="custom">自定义随机公式</param>
	/// <param name="seed">随机种子</param>
	/// <returns>中奖对象的索引</returns>
	public static int GetWinner (int[] store, CustomRandomFormula custom = null, int seed = -1) {
		ListKV<int, int> _store = new ListKV<int, int> ();
		for (int i = 0; i < store.Length; i++)
			_store.Add (i, store[i]);

		return GetWinner (_store, custom, seed);
	}

	/// <summary>
	/// 获得结果(为了较高的精准度使用万分比)
	/// </summary>
	/// <param name="probability">中奖概率</param>
	/// <param name="custom">自定义随机公式</param>
	/// <param name="seed">随机种子</param>
	/// <returns>true - 中奖; false - 未中奖</returns>
	public static bool GetResult (int probability, CustomRandomFormula custom = null, int seed = -1) {
		int target = GetRandom (0, 10001, custom, seed);
		Debug.LogFormat ("[Dice] random : {0}", target);
		if (target <= probability)
			return true;
		else
			return false;
	}

	/// <summary>
	/// 获得随机数
	/// </summary>
	/// <param name="min">最小值</param>
	/// <param name="max">最大值</param>
	/// <param name="custom">自定义随机公式</param>
	/// <param name="seed">随机种子</param>
	/// <returns></returns>
	private static int GetRandom (int min, int max, CustomRandomFormula custom = null, int seed = -1) {
		if (custom == null) {
			seed = seed < 0 ? (int) System.DateTime.Now.Ticks : seed;
			MersenneTwister mersenne = new MersenneTwister (seed);
			return mersenne.Next (min, max);
		} else {
			return custom (min, max, seed);
		}
	}
}