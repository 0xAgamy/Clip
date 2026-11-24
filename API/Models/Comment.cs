using System;

namespace Clips_api.Models;

public class Comment
{
    public int Id {get;set;}
    public int VideoId {get;set;}

    public string UserId{ get;set;}= string.Empty;
    public string Content {get;set;}= string.Empty;


    public DateTime CreatedAt {get;set;}= DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }



    
    // nav props
    public Video Video { get; set; } = null!;

    public User User { get; set; } = null!;

}
