using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace PartyInvites.Models
{
    public class GuestResponse
    {
        [Required(ErrorMessage="Please enter your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your email address")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage="Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please specify whether you'll attend")]
        public bool? WillAttend { get; set; }

        private MailMessage BuildMailMessage()
        {
            var msg = string.Format(@"Date: {0:yyyy-MM-dd hh:mm}
RSVP from: {1}
Email: {2}
Phone: {3}
Can come: {4}", DateTime.Now, Name, Email, Phone, WillAttend.Value ? "Yes" : "No");

            return new MailMessage("rsvp@example.com", "josh@6bit.com", Name + (WillAttend.Value ? " will attend" : " won't attend"), msg);
        }

        internal void Submit()
        {
            using (var client = new SmtpClient())
            using (var msg = BuildMailMessage())
                client.Send(msg);
        }
    }
}