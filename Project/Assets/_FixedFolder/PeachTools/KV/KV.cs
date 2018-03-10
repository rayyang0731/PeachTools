[System.Serializable]
public sealed class KV<K, V> {
	public K Key { get; private set; }

	public V Val { get; set; }

	public KV (K k, V v) {
		Key = k;
		Val = v;
	}
}