using System;
using System.Collections.Generic;
using System.Text;

namespace NdovuTisa.Models
{
    /// <summary>
    /// A quick on the fly web api response structure
    /// </summary>
    public class N9R
    {
        /// <summary>
        /// e.g 200,401,500
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// e.g You are a bad robot
        /// </summary>
        public string Message { get; set; }

        public N9R(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
