using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MsInUsa.Model
{
    [MetadataType(typeof(FullProfileMetaData))]
    public partial class FullProfile
    {
        public List<SelectListItem> itemslist {get; set;}
        public List<SelectListItem> listyear { get; set; }
        public List<SelectListItem> listpapers { get; set; }
        public List<SelectListItem> listterm { get; set; }
        public List<SelectListItem> listMonth { get; set; }
        public List<SelectListItem> listday { get; set; }
        public List<AdmitsReject> admitrejects { get; set; }
        public List<Universities_list> ambUniv { get; set; }
        public List<Universities_list> modUniv { get; set; }
        public List<Universities_list> safeUniv { get; set; }
        public string score { get; set; }
    }

    public class FullProfileMetaData
    {
        [Required(ErrorMessage ="Please enter the College Name")]
        public string UGCollege { get; set; }
        public string UGCourse { get; set; }
        [Required(ErrorMessage = "Please enter the Score")]
        public Nullable<decimal> UGScore { get; set; }
        public Nullable<int> UGBacklogs { get; set; }
        public Nullable<bool> AppearedGre { get; set; }
        public Nullable<decimal> GVerbal { get; set; }
        public Nullable<decimal> GQuant { get; set; }
        public Nullable<decimal> GAwa { get; set; }
        public Nullable<bool> Toefl { get; set; }
        public Nullable<bool> Ielts { get; set; }
        public Nullable<decimal> TIReading { get; set; }
        public Nullable<decimal> TIWriting { get; set; }
        public Nullable<decimal> TIListening { get; set; }
        public Nullable<decimal> TISpeaking { get; set; }
        public Nullable<decimal> WorkExperience { get; set; }
        public string Papers { get; set; }
        [Required(ErrorMessage = "Please enter the Course")]
        public string MSCourse { get; set; }
        public string Term { get; set; }
        public Nullable<decimal> Year { get; set; }
        public string DreamUniversity { get; set; }
        public Nullable<decimal> GDay { get; set; }
        public string GMonth { get; set; }
        public Nullable<decimal> GYear { get; set; }
    }
}