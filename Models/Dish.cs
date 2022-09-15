#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;



public class Dish {
    [Key]
    public int DishId {get; set;}
    [Required]
    public string Name {get; set;}
    [Required]
    public string Chef {get; set;}
    [Required]
    [Range(1,5)]
    public int Tastiness{get; set;}
    [Required]
    [MoreThan0]
    public int Calories{get; set;}
    [Required]
    public string Description {get;set;}
    public DateTime Created_At {get; set;}
    public DateTime Updated_At {get; set;}


}