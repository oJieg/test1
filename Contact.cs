namespace test1
{
    public class Contact
    {
        public string Name { get; set; }
        public string? Phone { get; set; }

        public Contact(string name, string? phone)
        {
            Name = name;
            Phone = phone;
        }

        public Contact(string name)
        {
            Name = name;
            Phone = "none";
        }
    }
}