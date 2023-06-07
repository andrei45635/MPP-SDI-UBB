namespace contest.domain
{
    public class Entity<TID>
    {
        public TID ID { get; set;}

        public Entity(TID id)
        {
            ID = id;
        }
    }
}