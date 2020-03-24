using System;


//  ===================
// 2020-03-24
//  ===================

namespace Pluto.netcoreTemplate.API.Models.Requests
{
    /// <summary>
    /// 对应 http put 更新 user
    /// </summary>
    public class PutUserRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool emailConfirmed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool phoneConfirmed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool lockoutEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int accessFailedCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string passwordHash { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }

    }
}