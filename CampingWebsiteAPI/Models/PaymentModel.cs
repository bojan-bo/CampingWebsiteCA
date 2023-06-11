using System;
using System.ComponentModel.DataAnnotations;

namespace CampingWebsiteAPI.Models
{
    public class PaymentModel
    {
        [Required]
        [CreditCard]
        public string CardNumber { get; set; }

        [Required]
        public string CardName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/yyyy}")]
        public DateTime ExpirationDate { get; set; }

        [Required]
        [Range(100, 9999, ErrorMessage = "Please enter a valid CVV number.")]
        public int CVV { get; set; }
    }
}

