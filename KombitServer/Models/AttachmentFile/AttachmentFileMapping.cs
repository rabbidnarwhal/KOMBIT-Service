using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KombitServer.Models {
  public partial class AttachmentFileMapping {
    public static AttachmentFile UpdateAttachmentFileMapping (AttachmentFile request, AttachmentFile response) {
      response.FileName = request.FileName;
      response.FilePath = request.FilePath;
      return response;
    }
  }
}