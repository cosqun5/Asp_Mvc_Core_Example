﻿using Bilet1.Utilities.Constans;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Bilet1.Areas.Admin.ViewModels;
public class UpdatePostVM
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Bos ola bilmez"), MaxLength(50, ErrorMessage = "Uzunlq maximum 50 simvol olmalidir")]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required, NotMapped]
    public IFormFile Photo { get; set; }
}
