using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KombitServer.Models
{
  public partial class FotoUpload
  {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string FotoName { get; set; }
    public string FotoPath { get; set; }

    public static FotoUpload FotoUploadMapping (ProductRequest productRequest, int id)
    {
      var fotoUpload = new FotoUpload ();
      fotoUpload.ProductId = id;
      fotoUpload.FotoName = productRequest.FotoName;
      fotoUpload.FotoPath = productRequest.FotoPath;
      return fotoUpload;
    }
  }
}