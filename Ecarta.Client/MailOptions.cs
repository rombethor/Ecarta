using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecarta.Client
{
    /// <summary>
    /// Options required for sending an email
    /// </summary>
    public class MailOptions
    {
        /// <summary>
        /// The recipient address
        /// </summary>
        [Required]
        public string Recipient { get; set; } = string.Empty;
        
        /// <summary>
        /// The subject
        /// </summary>
        [Required]
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// The content of the email, plain text
        /// </summary>
        [Required]
        public string Body { get; set; } = string.Empty;
    }
}
