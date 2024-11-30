using DataAccessLayer.Entities;
using System.Text.Json;

namespace DataAccessLayer.DataLoadingHelper
{
    public static class DataLoader<Entity> where Entity : BaseEntity
    {
        public static IEnumerable<Entity> GetData(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("Invalid file path");
                }
                else if (new FileInfo(filePath).Length == 0)
                {
                    return Enumerable.Empty<Entity>();
                }
                else
                {
                    using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        var users = JsonSerializer.Deserialize<IEnumerable<Entity>>(stream);
                        return users ?? Enumerable.Empty<Entity>();
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return Enumerable.Empty<Entity>();
            }

        }
    }
}
