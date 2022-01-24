namespace TotallyMoney.CustomerPreferenceCentre.Api.Models
{
    public class CustomerPreferenceReportItem
    {
        public DateTime Date { get; set; }
        public string[] Customers { get; set; } = null!;
    }
}
