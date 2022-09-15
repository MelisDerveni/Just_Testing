#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Request 
{
    [Key]
    public int RequestId {get;set;}
    public int SenderId {get;set;}
    
    
    public int ReciverId {get;set;}
    public user? Reciver{get;set;}
    // public user? Reciver{get;set;}
    
    public bool Accepted{get;set;}
    


    
    

}