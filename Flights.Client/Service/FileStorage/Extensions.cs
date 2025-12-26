using Microsoft.AspNetCore.Components.Forms;

namespace Flights.Client.Service.FileStorage;

public static class Extensions
{
    public static async Task<byte[]> ReadAllBytesAsync(this IBrowserFile file, int maxFileSize)
    {
        var readStream = file.OpenReadStream(maxFileSize);
  
        var buf = new byte[readStream.Length];
  
        var ms = new MemoryStream(buf);
  
        await readStream.CopyToAsync(ms);
  
        return ms.ToArray();
    }
}