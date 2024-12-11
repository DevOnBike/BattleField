using Logic.Security.AttributeBasedAccessControl;
using Logic.Security.AttributeBasedAccessControl.Model;
using Xunit.Abstractions;

namespace Tests.Security.AttributeBasedAccessControl
{
    public class AbacTests
    {
        [Fact]
        public void Execute()
        {
            // arrange
            // Create PDP instance
            var pdp = new PolicyDecisionPoint();

            // Define users
            var user1 = new User
            {
                Username = "john_doe",
                Attributes = new Dictionary<string, string>
                {
                    { "role", "admin" },
                    { "department", "HR" }
                }
            };

            var user2 = new User
            {
                Username = "jane_doe",
                Attributes = new Dictionary<string, string>
                {
                    { "role", "employee" },
                    { "department", "HR" }
                }
            };

            // Define resources
            var resource1 = new Resource
            {
                ResourceName = "PayrollData",
                Attributes = new Dictionary<string, string>
                {
                    { "owner", "HR" },
                    { "category", "confidential" }
                }
            };

            var resource2 = new Resource
            {
                ResourceName = "EmployeeHandbook",
                Attributes = new Dictionary<string, string>
                {
                    { "owner", "HR" },
                    { "category", "public" }
                }
            };

            // Define policies
            var policy1 = new Policy
            {
                Name = "AdminAccessToConfidentialResources",
                RequiredUserAttributes = new Dictionary<string, string>
                {
                    { "role", "admin" }
                },
                RequiredResourceAttributes = new Dictionary<string, string>
                {
                    { "category", "confidential" }
                },
                AccessDecision = (user, resource) =>
                {
                    return resource.Attributes["category"] == "confidential" && user.Attributes["role"] == "admin";
                }
            };

            var policy2 = new Policy
            {
                Name = "EmployeeAccessToPublicResources",
                RequiredUserAttributes = new Dictionary<string, string>
                {
                    { "role", "employee" }
                },
                RequiredResourceAttributes = new Dictionary<string, string>
                {
                    { "category", "public" }
                },
                AccessDecision = (user, resource) =>
                {
                    return resource.Attributes["category"] == "public" && user.Attributes["role"] == "employee";
                }
            };

            // Register policies in PDP
            pdp.RegisterPolicy(policy1);
            pdp.RegisterPolicy(policy2);

            // Create PEP instance and enforce access control
            var pep = new PolicyEnforcementPoint(pdp);

            // act

            // assert
            Assert.True(pep.RequestAccess(user1, resource1));
            Assert.True(pep.RequestAccess(user2, resource2));
            Assert.False(pep.RequestAccess(user2, resource1));
        }

        private readonly ITestOutputHelper _output;

        public AbacTests(ITestOutputHelper output)
        {
            _output = output;
        }
    }
}