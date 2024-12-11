namespace Logic.Security.AttributeBasedAccessControl.Model
{
    public class User
    {
        public string Username { get; set; }
        public Dictionary<string, string> Attributes { get; set; } // Key-value pairs for user attributes like "role", "department"
    }

    
}