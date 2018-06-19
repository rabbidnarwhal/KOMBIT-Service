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

    public static FotoUpload NewFotoUploadMapping (FotoUpload foto, int id)
    {
      var fotoUpload = new FotoUpload ()
      {
        ProductId = id,
        FotoName = foto.FotoName,
        FotoPath = foto.FotoPath
      };
      return fotoUpload;
    }
    public static FotoUpload UpdateFotoUploadMapping (FotoUpload updatedFoto, FotoUpload foto)
    {
      foto.FotoName = updatedFoto.FotoName;
      foto.FotoPath = updatedFoto.FotoPath;
      return foto;
    }
  }
}