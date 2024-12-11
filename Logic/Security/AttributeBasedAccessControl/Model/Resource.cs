namespace Logic.Security.AttributeBasedAccessControl.Model
{
    public class Resource
    {
        public string ResourceName { get; set; }
        public Dictionary<string, string> Attributes { get; set; } // Resource-specific attributes like "owner", "category"
    }    
}

