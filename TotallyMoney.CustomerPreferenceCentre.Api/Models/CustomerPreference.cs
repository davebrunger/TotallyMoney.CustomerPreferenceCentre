namespace TotallyMoney.CustomerPreferenceCentre.Api.Models
{
    public class CustomerPreference
    {
        public string Name { get; set; } = null!;
        public OneOf<int, DayOfWeek[], bool> Preference { get; set; }
    }
}
