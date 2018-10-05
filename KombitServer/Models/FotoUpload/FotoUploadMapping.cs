using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KombitServer.Models {
  public partial class FotoUploadMapping {
    public static FotoUpload UpdateFotoUploadMapping (FotoUpload request, FotoUpload response) {
      response.FotoName = request.FotoName;
      response.FotoPath = request.FotoPath;
      return response;
    }
  }
}