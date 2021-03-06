//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MsInUsa.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Universities_list
    {
        public Universities_list()
        {
            this.University_Deadlines = new HashSet<University_Deadlines>();
        }
    
        public int UnivId { get; set; }
        public string UnivName { get; set; }
        public string UnivType { get; set; }
        public Nullable<decimal> AcceptanceRate { get; set; }
        public Nullable<decimal> AvgTutionRate { get; set; }
        public Nullable<int> AvgLivingExpense { get; set; }
        public Nullable<decimal> AvgGreQuant { get; set; }
        public Nullable<int> TotalEnrollment { get; set; }
        public string Location { get; set; }
        public Nullable<decimal> ApplicationFee { get; set; }
        public string Website { get; set; }
        public string About { get; set; }
        public string LocationDetails { get; set; }
        public string Infrastructure { get; set; }
        public string ResidingOptions { get; set; }
        public string WeatherDetails { get; set; }
        public string Faculty_and_pedagogy { get; set; }
        public string Jobs_and_placements { get; set; }
        public string Crowd_and_campus_life { get; set; }
        public string Alumni { get; set; }
        public string Verdict { get; set; }
        public Nullable<decimal> WC_Jan_Mar { get; set; }
        public Nullable<decimal> WC_apr_jun { get; set; }
        public Nullable<decimal> WC_Jul_Sep { get; set; }
        public Nullable<decimal> WC_oct_dec { get; set; }
        public string State { get; set; }
        public string NickName { get; set; }
        public Nullable<decimal> Rank { get; set; }
        public string FinancialAid { get; set; }
        public Nullable<decimal> OverallScore { get; set; }
    
        public virtual ICollection<University_Deadlines> University_Deadlines { get; set; }
    }
}
