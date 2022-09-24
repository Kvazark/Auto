using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace Auto.Website.Models {
    public class OwnerDto {
        
        [HiddenInput(DisplayValue = false)]
        [Required]
        public string Name { get; set; }
        
        [Required]
        [DisplayName("number of phone")]
        public string NumberPhone { get; set; }

        [Required]
        [DisplayName("Address")]
        public string Address { get; set; }
        
        private string registration;

        private static string NormalizeRegistration(string reg) {
            return reg == null ? reg : Regex.Replace(reg.ToUpperInvariant(), "[^A-Z0-9]", "");
        }

        [Required]
        [DisplayName("Registration of the belonging car")]
        public string RegistrationCode {
            get => NormalizeRegistration(registration);
            set => registration = value;
        }
   
    }
}