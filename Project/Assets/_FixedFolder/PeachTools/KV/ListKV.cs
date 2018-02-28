using System;
using System.Collections;
using System.Collections.Generic;

public class ListKV<K, V> {
    private List<KV<K, V>> _list;

    public ListKV () {
        _list = new List<KV<K, V>> ();
    }

    public V this [K key] {
        get { return _find (key).Val; }
        set { _find (key).Val = value; }
    }

    private KV<K, V> _find (K key) {
        for (int i = 0; i < _list.Count; i++) {
            if (_list[i].Key.Equals (key))
                return _list[i];
        }
        return null;
    }

    public KV<K, V> this [int index] {
        get {
            if (index < 0)
                throw new System.Exception ("Index 不能小于0");
            else
                return _list[index];
        }
        set {
            _list[index] = value;
        }
    }

    public int Count { get { return _list.Count; } }

    public void Add (KV<K, V> kv) {
        _list.Add (kv);
    }

    public void Add (K k, V v) {
        _list.Add (new KV<K, V> (k, v));
    }

    public void Clear () {
        _list.Clear ();
    }

    public bool ContainKey (K key) {
        return _find (key) != null;
    }

    public bool Contains (KV<K, V> kv) {
        return _list.Contains (kv);
    }

    public int IndexOf (K key) {
        for (int i = 0; i < _list.Count; i++) {
            if (_list[i].Key.Equals (key))
                return i;
        }
        throw new Exception (string.Format ("Key:{0} 不存在", key.ToString ()));
    }

    public void Insert (int index, KV<K, V> value) {
        _list.Insert (index, value);
    }

    public void Remove (KV<K, V> value) {
        _list.Remove (value);
    }

    public void RemoveAt (int index) {
        _list.RemoveAt (index);
    }

    public void RemoveKey (K key) {
        RemoveAt (IndexOf (key));
    }
}