using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DynamicHeaders
{
	private List<DynamicHeader> Headers;

	private int HeaderRows;

	private int HeaderCols;

	public DynamicHeaders(string Header)
	{
		Headers = new List<DynamicHeader>();
		string[] array = Header.Split(',');
		string[] array2 = array;
		foreach (string header in array2)
		{
			Headers.Add(new DynamicHeader(header));
		}
		HeaderCols = Headers.Count;
		HeaderRows = Headers.Max((DynamicHeader H) => H.HeaderDepth);
		ParseHeader();
	}

	public ArrayList ParseHeader()
	{
		Array array = Array.CreateInstance(typeof(DynamicHeaderCell), HeaderRows, HeaderCols);
		for (int i = 0; i < Headers.Count; i++)
		{
			DynamicHeader dynamicHeader = Headers[i];
			for (int j = 0; j < dynamicHeader.Headers.Length; j++)
			{
				string header = dynamicHeader.Headers[j];
				array.SetValue(new DynamicHeaderCell(header), j, i);
			}
		}
		for (int i = 0; i < HeaderRows; i++)
		{
			for (int j = 0; j < HeaderCols; j++)
			{
				DynamicHeaderCell dynamicHeaderCell = (DynamicHeaderCell)array.GetValue(i, j);
				if (dynamicHeaderCell == null)
				{
					continue;
				}
				for (int k = j + 1; k < HeaderCols; k++)
				{
					DynamicHeaderCell dynamicHeaderCell2 = (DynamicHeaderCell)array.GetValue(i, k);
					if (dynamicHeaderCell2 != null)
					{
						if (!dynamicHeaderCell.Header.Equals(dynamicHeaderCell2.Header))
						{
							break;
						}
						dynamicHeaderCell.ColSpan++;
						array.SetValue(null, i, k);
					}
				}
			}
		}
		for (int j = 0; j < HeaderCols; j++)
		{
			for (int i = 0; i < HeaderRows; i++)
			{
				DynamicHeaderCell dynamicHeaderCell = (DynamicHeaderCell)array.GetValue(i, j);
				if (dynamicHeaderCell == null)
				{
					continue;
				}
				for (int k = i + 1; k < HeaderRows; k++)
				{
					DynamicHeaderCell dynamicHeaderCell2 = (DynamicHeaderCell)array.GetValue(k, j);
					if (dynamicHeaderCell2 == null)
					{
						dynamicHeaderCell.RowSpan++;
						array.SetValue(null, k, j);
						continue;
					}
					break;
				}
			}
		}
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < HeaderRows; i++)
		{
			List<DynamicHeaderCell> list = new List<DynamicHeaderCell>();
			for (int j = 0; j < HeaderCols; j++)
			{
				DynamicHeaderCell dynamicHeaderCell3 = (DynamicHeaderCell)array.GetValue(i, j);
				if (dynamicHeaderCell3 != null)
				{
					list.Add(dynamicHeaderCell3);
				}
			}
			arrayList.Add(list);
		}
		return arrayList;
	}
}
