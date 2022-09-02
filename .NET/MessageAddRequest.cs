using Sabio.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Messages
{
    public class MessageAddRequest
    {
       
        [Required]
        [MinLength(2)]
        public string MessageText { get; set; }

        [MinLength(2)]
        public string Subject { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)]
        public int RecipientId { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)]
        public int SenderId { get; set; }

        public UserProfileBase Sender { get; set; }

        public DateTime DateSent { get; set; }

        public DateTime DateRead { get; set; }

      
    }
}
