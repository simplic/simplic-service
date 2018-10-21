﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.DataStack
{
    /// <summary>
    /// Flow event args
    /// </summary>
    public class ReportFlowEventArgs
    {
        /// <summary>
        /// Gets or sets the stack id
        /// </summary>
        public Guid StackId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the stack name
        /// </summary>
        public Guid StackName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the instance data id
        /// </summary>
        public Guid InstanceDataId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the event payload
        /// </summary>
        public object Payload
        {
            get;
            set;
        }
    }
}