using Logic.Security.AttributeBasedAccessControl.Model;

namespace Logic.Security.AttributeBasedAccessControl
{
    /// <summary>
    /// The PDP is responsible for evaluating the policies and making a decision.
    /// </summary>
    public class PolicyDecisionPoint
    {
        private readonly List<Policy> _policies = [];

        // Register policies
        public void RegisterPolicy(Policy policy)
        {
            _policies.Add(policy);
        }

        // Evaluate access request based on the policies
        public bool Evaluate(User user, Resource resource)
        {
            foreach (var policy in _policies)
            {
                if (EvaluatePolicy(user, resource, policy))
                {
                    return true; // If any policy grants access, return true
                }
            }

            return false; // If no policy grants access, return false
        }

        // Evaluate individual policy
        private bool EvaluatePolicy(User user, Resource resource, Policy policy)
        {
            // Check if user's attributes match the required attributes for the policy
            foreach (var requiredUserAttribute in policy.RequiredUserAttributes)
            {
                if (!user.Attributes.ContainsKey(requiredUserAttribute.Key) ||
                    user.Attributes[requiredUserAttribute.Key] != requiredUserAttribute.Value)
                {
                    return false;
                }
            }

            // Check if resource's attributes match the required attributes for the policy
            foreach (var requiredResourceAttribute in policy.RequiredResourceAttributes)
            {
                if (!resource.Attributes.ContainsKey(requiredResourceAttribute.Key) ||
                    resource.Attributes[requiredResourceAttribute.Key] != requiredResourceAttribute.Value)
                {
                    return false;
                }
            }

            // Run the access decision function for the policy
            return policy.AccessDecision(user, resource);
        }
    }
}

