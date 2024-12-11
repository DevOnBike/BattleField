namespace Logic.Security.AttributeBasedAccessControl.Model
{
    public class Attributes
    {
        private readonly Dictionary<string, string> _attributes = [];

        // Add or update an attribute
        public void Add(string key, string value)
        {
            _attributes[key] = value;
        }

        // Retrieve an attribute value (throws exception if key is not found)
        public string Get(string key)
        {
            return _attributes.GetValueOrDefault(key);
        }

        // Check if an attribute exists
        public bool Contains(string key)
        {
            return _attributes.ContainsKey(key);
        }
    }
}

