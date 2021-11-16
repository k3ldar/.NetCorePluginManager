using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;

namespace AspNetCore.PluginManager.Handlers
{
    /// <summary>
    /// Api Authorization handler for managing access to Api's
    /// </summary>
    public sealed class ApiAuthorizationHandler : AuthorizationHandler<ApiAuthorizationHandler>, IAuthorizationRequirement
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiAuthorizationHandler()
        {

        }

        #endregion Constructors

        #region Public Methods

        public override Task HandleAsync(AuthorizationHandlerContext context)
        {
            return base.HandleAsync(context);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiAuthorizationHandler requirement)
        {
            throw new NotImplementedException();
        }

        #endregion Protected Methods
    }
}
