//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace testRAF.Models.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class izlenenler
    {
        public int IzId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string imdb_id { get; set; }
        public string title { get; set; }
        public string poster_path { get; set; }
        public Nullable<int> id { get; set; }
    
        public virtual User User { get; set; }
    }
}