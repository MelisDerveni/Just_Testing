#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;


namespace Just_Testing.Models;
public class Dish2 {
    [Key]
    public int DishId {get; set;}
    [Required]
    public string Name {get; set;}
    
    
    [Required]
    [Range(1,5)]
    public int Tastiness{get; set;}
    [Required]
    public int Calories{get; set;}
    [Required]
    public string Description {get;set;}
    public int ChefId{get;set;}
    public Chef? Cook {get;set;}
    public DateTime Created_At {get; set;}
    public DateTime Updated_At {get; set;}


}