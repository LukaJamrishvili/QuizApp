using DataAccessLayer.Entities;
using System.Text.Json;

namespace DataAccessLayer.DataLoadingHelper
{
    public static class DataSaver<Entity> where Entity : BaseEntity
    {
        public static void SaveData(string filePath, IEnumerable<Entity> data)
        {
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                JsonSerializer.Serialize(stream, data, new JsonSerializerOptions { WriteIndented = true });
            }
        }
    }
}
