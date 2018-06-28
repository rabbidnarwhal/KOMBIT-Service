using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KombitServer.Models
{
  public partial class MKabKota
  {
    public int Id { get; set; }
    public int ProvinsiId { get; set; }
    public string Name { get; set; }

    [ForeignKey ("ProvinsiId")]
    public MProvinsi Provinsi { get; set; }
  }
}