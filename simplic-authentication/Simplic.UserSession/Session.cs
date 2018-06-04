﻿using System.Collections.Generic;

namespace Simplic.UserSession
{
    /// <summary>
    /// An object holding information about the current logged in user 
    /// </summary>
    public class Session
    {
        /// <summary>
        /// Gets the currently logged in user id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets the currently logged in user name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets the currently logged in users access groups
        /// </summary>
        public IList<int> UserAccessGroups { get; set; }

        /// <summary>
        /// Gets the currently logged in users id as bit mask
        /// </summary>
        public string UserBitMask { get; set; }

        /// <summary>
        /// Gets the currently logged in users access groups ids as bit mask
        /// </summary>
        public string UserAccessGroupsBitMask { get; set; }

        /// <summary>
        /// Gets the currently logged in users role (wether its a super user or not)
        /// </summary>
        public bool IsSuperUser { get; set; }

        /// <summary>
        /// Gets or sets whether the current user is an active directory user
        /// </summary>
        public bool IsADUser { get; set; }
    }
}