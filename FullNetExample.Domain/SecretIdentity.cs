namespace FullNetExample.Domain
{
    public class SecretIdentity : ClientChangeTracker
    {
        public int Id { get; set; }
        public string RealName { get; set; }
        public Samurai Smaurai { get; set; }
        public int SamuraiId { get; set; }
    }
}
