public class ContactRequest
{
    public string NameCutomer { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string EmailCustomer { get; set; } = string.Empty;
    public DateTime DataWedding { get; set; } = DateTime.Now;
    public string Note { get; set; } = string.Empty;
}


public class ContactRespone
{
    public bool Status { get; set; } = false;
    public string Message { get; set; } = string.Empty;
}