using System;
using System.Collections.Generic;

namespace KombitServer.Models {
    public partial class AttachmentFile {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public AttachmentFile () { }
        public AttachmentFile (AttachmentFile file, int productId) {
            ProductId = productId;
            FileName = file.FileName;
            FilePath = file.FilePath;
        }
    }
}