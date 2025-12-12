using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class Report
{
	public string Title { get; set; } = string.Empty;
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public List<DailySummary> Summaries { get; set; } = new();

	public void ExportToCSV(string path)
	{
		var lines = Summaries.Select(s => $"{s.Date:yyyy-MM-dd},{s.TotalEaten},{s.TotalBurned},{s.NetCalories}");
		File.WriteAllLines(path, new[] { "Date,TotalEaten,TotalBurned,NetCalories" }.Concat(lines));
	}

	public void ExportToPDF(string path)
	{
		// Placeholder: implement with a PDF library like iTextSharp or PdfSharp
	}
}
