namespace DataAccessLayer.Entities
{
    public abstract class BaseEntity
    {
        protected int _id;
        protected BaseEntity(int id)
        {
            _id = id;
        }
    }
}