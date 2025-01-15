using System.Text;
using Spire.Doc;
using Spire.Doc.Documents;

namespace TagCloudDI.Data
{
    public class DocFileDataSource : IFileDataSource
    {
        public string GetData(string filePath)
        {
            var text = new StringBuilder();
            var doc = new Document();
            doc.LoadFromFile(filePath);

            foreach (Section section in doc.Sections)
            {
                foreach (Paragraph paragraph in section.Paragraphs)
                {
                    text.AppendLine(paragraph.Text);
                }
            }

            return text.ToString().Trim();
        }
    }

}
