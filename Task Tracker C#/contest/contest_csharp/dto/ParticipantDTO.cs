namespace contest_csharp.dto
{
    public class ParticipantDTO
    {
        public string name { get; set; }
        public int age { get; set; }

        public ParticipantDTO(string name, int age)
        {
            this.name = name;
            this.age = age;
        }
    }
}
