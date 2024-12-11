namespace Logic.Security.AttributeBasedAccessControl.Model
{
    public class Policy
    {
        public string Name { get; set; }
        public Dictionary<string, string> RequiredUserAttributes { get; set; } // Required user attributes
        public Dictionary<string, string> RequiredResourceAttributes { get; set; } // Required resource attributes
        public Func<User, Resource, bool> AccessDecision { get; set; } // Function to decide access
    }
}

