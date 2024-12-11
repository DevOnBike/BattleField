using Logic.Security.AttributeBasedAccessControl.Model;

namespace Logic.Security.AttributeBasedAccessControl
{
    /// <summary>
    /// The PEP is the component that intercepts access requests, queries the PDP for an access decision, and then enforces it
    /// </summary>
    public class PolicyEnforcementPoint
    {
        private PolicyDecisionPoint _pdp;

        public PolicyEnforcementPoint(PolicyDecisionPoint pdp)
        {
            _pdp = pdp;
        }

        // Intercept access requests and ask PDP for an access decision
        public bool RequestAccess(User user, Resource resource)
        {
            return _pdp.Evaluate(user, resource);
        }
    }
}