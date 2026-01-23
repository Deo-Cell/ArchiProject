public class TacosModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sauce { get; set; } = string.Empty;
    public string Meat { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public bool IsVegan { get; set; }
}