//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace sinavproje.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ilceler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ilceler()
        {
            this.dbmusteris = new HashSet<dbmusteri>();
        }
    
        public int ilceId { get; set; }
        public string ilceAdi { get; set; }
        public Nullable<int> sehirId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<dbmusteri> dbmusteris { get; set; }
        public virtual iller iller { get; set; }
    }
}
