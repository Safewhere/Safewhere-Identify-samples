namespace Safewhere.Samples.RestApi.Domain
{
	public static class RequestObject
	{
        #region Connections

        public static readonly string Certificate = "certificates";

        #endregion

		#region Connections

		public static readonly string Connections = "connections";
	    public static readonly string ConnectionsCertificate = "connections/certificate";
        public static readonly string ConnectionsClaimTransformation = "connections/transformation";
        public static readonly string ConnectionsClaimTransformations = "connections/transformations";

		#endregion

		#region Users

		public static readonly string Users = "users";
        public static readonly string MassUsers = "users/.batch";
        public static readonly string UsersSearch = "users/.search ";

        #endregion

        #region Organizations

        public static readonly string Organizations = "organizations";
		public static readonly string OrganizationsMany = "organizations/many";
		public static readonly string OrganizationsChilds = "organizations/{0}/childs";

		#endregion

		#region Groups

		public static readonly string Groups = "groups";
		public static readonly string GroupsMany = "groups/many";

		#endregion

        public const string ClaimTransformations = "transformations";
        public static readonly string ClaimTransformationExcludeIdentify = ClaimTransformations + "/excludeIdentify";

        public const string SystemSetup = "systemsetup";

        #region ClaimDefinitions
        public static readonly string ClaimDefinitions = "claimdefinitions";
        public static readonly string Ids = "ids";
        public static readonly string Many = "many";
        public static readonly string ClaimTypes = "claimtypes";
        public static readonly string ClaimDefinitionsSearch = "claimdefinitions/.search";
        public static readonly string CountSpecification = "countSpecification";
        public static readonly string ClaimDefinitionsMany = "claimdefinitions/many";
        public static readonly string ClaimDefinitionsFilter = "claimdefinitions/.filter";
        public static readonly string ActivationTimes = "activationtimes";
        #endregion
    }
}
