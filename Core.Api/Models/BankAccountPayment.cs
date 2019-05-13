namespace Core.Api.Models
{
    public class BankAccountPayment
    {
        public bool receive_cash_payment { get; set; }
        public bool receive_online_payment { get; set; }
        public bool recieve_money_transfer { get; set; }
        public string BankName { get; set; }
        public string IBankNumber { get; set; }
    }
}