using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KombitServer.Models {
  public partial class AttachmentFileMapping {
    public static AttachmentFile UpdateAttachmentFileMapping (AttachmentFile request, AttachmentFile existing) {
      existing.FileName = request.FileName;
      existing.FilePath = request.FilePath;
      return existing;
    }
  }
}