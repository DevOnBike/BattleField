namespace Logic.Security.AttributeBasedAccessControl.Model
{
    // Represents an attribute store (key-value pairs)
    public class AccessRequest
    {
        public Attributes Subject { get; set; } // User or system making the request
        public Attributes Resource { get; set; } // Resource being accessed
        public Attributes Environment { get; set; } // Environment attributes (time, location, etc.)
        
        public string Action { get; set; } // Action to perform (e.g., "read", "write")
    }
    
}

