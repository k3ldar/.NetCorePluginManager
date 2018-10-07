using System;

namespace SharedPluginFeatures
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class DenySpiderAttribute : Attribute
    {
        #region Constructors

        public DenySpiderAttribute()
            : this ("*")
        {

        }

        public DenySpiderAttribute(string userAgent)
            : this (userAgent, String.Empty)
        {

        }

        public DenySpiderAttribute(string userAgent, string comment)
        {
            if (String.IsNullOrEmpty(userAgent))
                throw new ArgumentNullException(nameof(userAgent));

            UserAgent = userAgent;
            Comment = comment;
        }

        #endregion Constructors

        #region Properties

        public string UserAgent { get; private set; }

        public string Comment { get; private set; }

        #endregion Properties
    }
}
