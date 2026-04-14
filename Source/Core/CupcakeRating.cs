public class CupcakeRating
{
    public string ImagePath { get; set; } = "";
    public int Stars { get; set; } = 0;
    public float Coverage { get; set; } = 0;
    public float Variety { get; set; } = 0;
    public float Distribution { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}