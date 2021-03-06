﻿using System;

namespace Simplic.DataStack
{
    /// <summary>
    /// Flow event configuration
    /// </summary>
    public class StackReportFlowEvent
    {
        /// <summary>
        /// Gets or sets the flow identification
        /// </summary>
        public string FlowIdentification
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the display name
        /// </summary>
        public string DisplayName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the icon name
        /// </summary>
        public Guid IconId
        {
            get;
            set;
        }
    }
}
