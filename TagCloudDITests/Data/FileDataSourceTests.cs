using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagCloudDI.Data;

namespace TagCloudDITests.Data
{
    public class FileDataSourceTests
    {
        private const string RelativePath = "..\\..\\..\\Data\\ForCheck";
        private const string CorrectText = "Всё работает корректно!";

        [Test]
        public void TxtFileDataSource_ShouldReadFileCorrect_WhenExtensionIsTxt()
        {
            var source = new TxtFileDataSource();
            var extension = ".txt";

            var result = source.GetData(RelativePath + extension);

            result.Should().Be(CorrectText);
        }

        [Test]
        public void DocFileDataSource_ShouldReadFileCorrect_WhenExtensionIsDoc()
        {
            var source = new DocFileDataSource();
            var extension = ".doc";

            var result = source.GetData(RelativePath + extension);

            result.Should().Be(CorrectText);
        }

        [Test]
        public void DocFileDataSource_ShouldReadFileCorrect_WhenExtensionIsDocx()
        {
            var source = new DocxFileDataSource();
            var extension = ".docx";

            var result = source.GetData(RelativePath + extension);

            result.Should().Be(CorrectText);
        }
    }
}
