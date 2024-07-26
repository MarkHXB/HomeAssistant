namespace HomeAssistant.Forms
{
    public partial class MoneyTrackingForm
    {
        public class Category
        {
            public Category()
            {
                Name = "Default Category";          
            }
            public Category(string name, bool isChecked)
            {
                Name = name;
                IsChecked = isChecked;
            }

            public string Name { get; set; }
            public bool IsChecked { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
