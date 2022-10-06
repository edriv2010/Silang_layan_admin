using System.Collections;

public class TwoArrayList
{
	public ArrayList ArrayList1 = new ArrayList();

	public ArrayList ArrayList2 = new ArrayList();

	public void Add(string FirstValue, object SecondValue)
	{
		ArrayList1.Add(FirstValue);
		ArrayList2.Add(SecondValue);
	}

	public void RemoveAt(int Indeks)
	{
		if (Indeks > -1)
		{
			ArrayList1.RemoveAt(Indeks);
			ArrayList2.RemoveAt(Indeks);
		}
	}

	public void Remove(string FirstKeyValue)
	{
		int num = ArrayList1.IndexOf(FirstKeyValue);
		if (num > -1)
		{
			ArrayList1.RemoveAt(num);
			ArrayList2.RemoveAt(num);
		}
	}

	public void Clear()
	{
		ArrayList1.Clear();
		ArrayList2.Clear();
	}

	public string Item1(int Index)
	{
		return ArrayList1[Index].ToString();
	}

	public object Item2(int Index)
	{
		return ArrayList2[Index];
	}

	public int Count()
	{
		return ArrayList1.Count;
	}

	public void SetItem1(int Index, object Value)
	{
		ArrayList1[Index] = Value;
	}

	public void SetItem2(int Index, object Value)
	{
		ArrayList2[Index] = Value;
	}
}
