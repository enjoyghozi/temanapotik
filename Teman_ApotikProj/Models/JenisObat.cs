//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Teman_ApotikProj.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class JenisObat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public JenisObat()
        {
            this.Obat = new HashSet<Obat>();
        }

        
        public int Id_Jenis_Obat { get; set; }

        [StringLength(20, ErrorMessage = "Minimal {0} adalah dan harus {2} panjang karakter", MinimumLength = 4)]
        [Required(ErrorMessage = "Jenis Obat wajib ditulis")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Masukan hanya huruf dan angka !")]
        public string Jenis_Obat { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Obat> Obat { get; set; }
    }
}
