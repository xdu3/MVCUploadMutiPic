using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace uploadMutiPic.Models
{
    public class FileModel
    {
        [Required(ErrorMessage ="Please select file.")]
        [Display(Name ="Browse File")]
        public IFormFile files { get; set; }
    }
}
