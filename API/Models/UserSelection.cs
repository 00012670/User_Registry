using System.ComponentModel.DataAnnotations;

public class UserSelection
{
    [Required]
    public bool? IsSelected { get; set; }
}