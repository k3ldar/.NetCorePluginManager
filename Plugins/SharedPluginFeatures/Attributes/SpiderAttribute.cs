using System;

namespace SharedPluginFeatures
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class SpiderAttribute : Attribute
    {
        #region Constructors

        public SpiderAttribute()
            : this ("*", false)
        {

        }

        public SpiderAttribute(in string userAgent, in bool allowSpider)
            : this (userAgent, allowSpider, String.Empty)
        {

        }

        public SpiderAttribute(in string userAgent, in bool allowSpider, in string comment)
        {
            if (String.IsNullOrEmpty(userAgent))
                throw new ArgumentNullException(nameof(userAgent));

            UserAgent = userAgent;
            AllowSpider = allowSpider;
            Comment = comment;
        }

        #endregion Constructors

        #region Properties

        public bool AllowSpider { get; private set; }

        public string UserAgent { get; private set; }

        public string Comment { get; private set; }

        #endregion Properties
    }
}
