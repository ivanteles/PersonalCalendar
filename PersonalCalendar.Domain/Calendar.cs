namespace PersonalCalendar.Domain
{
    public class Calendar : Entity
    {
        public string Name { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
