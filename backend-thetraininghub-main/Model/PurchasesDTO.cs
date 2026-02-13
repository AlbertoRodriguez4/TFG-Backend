public class PurchaseDTO
{
    public int PurchaseId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public string ItemType { get; set; }
    public int ItemBonus { get; set; }
    public int ItemPrice { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string? ImageUrl { get; set; } 
}
