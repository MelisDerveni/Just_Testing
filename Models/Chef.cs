using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618
namespace Just_Testing.Models;
public class Chef 
{
    [Key]
    public int ChefId {get;set;}

    [Required]
    public string ChefFirstName {get; set;}
    [Required]
    public string ChefLastName {get; set;}
    [Required]
    public DateTime DateOfBirth {get; set;}
    
    public List<Dish2> CreatedDishes { get; set; } = new List<Dish2>(); 
}