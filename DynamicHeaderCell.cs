public class DynamicHeaderCell
{
	public string Header { get; set; }

	public int RowSpan { get; set; }

	public int ColSpan { get; set; }

	public DynamicHeaderCell(string header)
	{
		RowSpan = 1;
		ColSpan = 1;
		Header = header;
	}
}
