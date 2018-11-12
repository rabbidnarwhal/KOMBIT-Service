using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KombitServer.Models {
  public partial class FotoUpload {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string FotoName { get; set; }
    public string FotoPath { get; set; }
    public string UseCase { get; set; }
    public string Title { get; set; }
    public FotoUpload () { }
    public FotoUpload (FotoUpload foto, int productId) {
      ProductId = productId;
      FotoName = foto.FotoName;
      FotoPath = foto.FotoPath;
      UseCase = foto.UseCase;
      Title = foto.Title;
    }
  }
}