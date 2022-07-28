using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;

namespace AspNetCore.PluginManager.Handlers
{
    /// <summary>
    /// Api Authorization handler for managing access to Api's
    /// </summary>
    public sealed class ApiAuthorizationHandler1 : AuthorizationHandler<ApiAuthorizationHandler1>, IAuthorizationRequirement
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiAuthorizationHandler1()
        {

        }

		/// <summary>
		/// Handle requirements async method
		/// </summary>
		/// <param name="context"></param>
		/// <param name="requirement"></param>
		/// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiAuthorizationHandler1 requirement)
        {
            context.Succeed(requirement);

            return Task.CompletedTask;
        }


        #endregion Constructors

        #region Public Methods


        #endregion Public Methods
    }
}
