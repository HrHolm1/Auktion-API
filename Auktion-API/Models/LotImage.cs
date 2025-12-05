namespace Auktion_API.Models;

public class LotImage
{
    public int Id { get; set; }

    public int LotId { get; set; }
    public Lot Lot { get; set; } = null!;

    public string FileName { get; set; } = string.Empty;   // stored file name
    public string Url { get; set; } = string.Empty;        // public URL (e.g. /uploads/lots/1/xyz.jpg)
}