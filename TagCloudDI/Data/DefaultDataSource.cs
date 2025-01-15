namespace TagCloudDI.Data
{
    internal class DefaultDataSource : IFileDataSource
    {
        public string GetData(string filePath)
        {
            byte[] bytes = File.ReadAllBytes(filePath);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}
