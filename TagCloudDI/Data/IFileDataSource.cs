namespace TagCloudDI.Data
{
    public interface IFileDataSource
    {
        public string GetData(string filePath);
    }
}