﻿using static HomeAssistant.Forms.MoneyTrackingForm;

namespace HomeAssistant.Forms
{

    public class TransactionRecordJson
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public int Field3 { get; set; }
        public string Currency { get; set; }
        public string Date1 { get; set; }
        public string Date2 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Vendor { get; set; }
        public string Description { get; set; }
        public string Field11 { get; set; }
        public string Field12 { get; set; }
        public string Field13 { get; set; }
        public string TransactionType { get; set; }
        public string Field15 { get; set; }
        public List<Category> Categories { get; set; }

        public override string ToString()
        {
            return $"{Vendor} | {Field3} {Currency}";
        }
    }


}
