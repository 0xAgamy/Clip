using System;

namespace Clips.Domain.Models;

public class Game
{
   
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? CoverKey { get; set; }

    public ICollection<Video> Videos { get; set; } = new List<Video>();

}
