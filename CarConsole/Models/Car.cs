namespace CarConsole.Models
{
     public record Car
     {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name {get; set;} = "default";
     }
}

