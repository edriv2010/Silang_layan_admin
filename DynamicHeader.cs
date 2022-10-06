public class DynamicHeader
{
	public int HeaderDepth { get; set; }

	public string[] Headers { get; set; }

	public DynamicHeader(string header)
	{
		Headers = header.Split('|');
		HeaderDepth = Headers.Length;
	}
}
