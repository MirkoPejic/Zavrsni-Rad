//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Zavrsni_Rad.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Colloquium
    {
        public int ColloquiumID { get; set; }
        public int CourseID { get; set; }
        public int LecturerID { get; set; }
        public System.DateTime ColloquiumDate { get; set; }
        public System.TimeSpan ColloquiumStart { get; set; }
        public string ColloquiumText { get; set; }
    
        public virtual Course Course { get; set; }
        public virtual LecturerPerson LecturerPerson { get; set; }
    }
}
