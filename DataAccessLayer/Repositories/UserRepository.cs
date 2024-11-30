using DataAccessLayer.DataLoadingHelper;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using System.Text.Json;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _filePath;
        private List<User> _users;

        public UserRepository(string filePath)
        {
            _filePath = filePath;
            _users = DataLoader<User>.GetData(_filePath).ToList();
        }

        public bool Contains(string username) => _users.Select(u => u.Username).Contains(username);

        public User Create(User newEntity)
        {
            _users.Add(newEntity);
            DataSaver<User>.SaveData(_filePath, _users);
            return newEntity;
        }

        public void Delete(int id)
        {
            User target = _users.FirstOrDefault(u => u.Id == id);
            if (target != null)
            {
                _users.Remove(target);
                DataSaver<User>.SaveData(_filePath, _users);
            }
        }

        public IEnumerable<User> GetAll() => _users;

        public User GetWithId(int id) => _users.FirstOrDefault(u => u.Id == id);

        public User GetWithUsername(string username) => _users.FirstOrDefault(u => u.Username == username);
        public User Update(User entity)
        {
            _users[_users.FindIndex(u => u.Id == entity.Id)] = entity;
            DataSaver<User>.SaveData(_filePath, _users);
            return entity;
        }
    }
}
