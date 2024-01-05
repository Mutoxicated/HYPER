using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDict<TKey, TValue> : IEnumerable
{
    [SerializeField]
	private List<TKey> keysList = new List<TKey>();
	public List<TKey> KeysList
	{
		get{return keysList;}
		set{keysList=value;}
	}

	[SerializeField]
	private List<TValue> valuesList = new List<TValue>();
	public List<TValue> ValuesList
	{
		get{return valuesList;}
		set{valuesList=value;}
	}

    private Dictionary<TKey, TValue> dictionaryData = new Dictionary<TKey, TValue>();
	public Dictionary<TKey, TValue> DictionaryData
	{
		get{return dictionaryData;}
		set{dictionaryData =value;}
	}

    public IEnumerator GetEnumerator()
    {
        return dictionaryData.GetEnumerator();
    }

    public void Add(TKey key, TValue value){
        keysList.Add(key);
        ValuesList.Add(value);
    }
}
