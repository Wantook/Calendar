// Event.cs
namespace Randomize.Model
{
    public class Event
    {
        public DateTime Date { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
    }
}
