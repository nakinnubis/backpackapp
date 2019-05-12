namespace Core.Api.Models
{
    public class BankAccountPayment
    {
        public bool isCash { get; set; }
        public bool isOnlinePayment { get; set; }
        public bool Transfer { get; set; }
        public string BankName { get; set; }
        public string IBankNumber { get; set; }
    }
}