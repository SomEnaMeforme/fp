using System.Text;
using Xceed.Words.NET;
namespace TagCloudDI.Data
{
    public class DocxFileDataSource : IFileDataSource
    {
        public string GetData(string filePath)
        {
            var text = new StringBuilder();
            using (var document = DocX.Load(filePath))
            {
                text.Append(document.Text);
            }
            return text.ToString();
        }
    }
}
