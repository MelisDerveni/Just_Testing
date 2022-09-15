#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class user {
    [Key]
    public int id {get; set;}
    [Required]
    [MinLength(2)]
    public string username {get; set;}
    [Required]
    [EmailAddress]
    
    public string email  {get; set;}
    [Required]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
    public string password {get; set;}

    [InverseProperty("Sender")]
    public List<Request> RequestsSent {get;set;} = new List<Request>();
    
    [InverseProperty("Reciver")]
    public List<Request> RequestsReciverd {get;set;} = new List<Request>();


    [NotMapped]
    [Required]
    [Compare("password")]
    [DataType(DataType.Password)]
    public string Confirm { get; set; } 
    public DateTime create_time {get; set;}

}