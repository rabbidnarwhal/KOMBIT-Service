using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KombitServer.Models
{
  public partial class KombitDBContext : DbContext
  {
    public virtual DbSet<FotoUpload> FotoUpload { get; set; }
    public virtual DbSet<Interaction> Interaction { get; set; }
    public virtual DbSet<MCategory> MCategory { get; set; }
    public virtual DbSet<MCompany> MCompany { get; set; }
    public virtual DbSet<MHolding> MHolding { get; set; }
    public virtual DbSet<MKabKota> MKabKota { get; set; }
    public virtual DbSet<MProvinsi> MProvinsi { get; set; }
    public virtual DbSet<MTypeId> MTypeId { get; set; }
    public virtual DbSet<MUser> MUser { get; set; }
    public virtual DbSet<Product> Product { get; set; }
    public virtual DbSet<Notification> Notification { get; set; }
    public virtual DbSet<SysParam> SysParam { get; set; }

    public KombitDBContext (DbContextOptions<KombitDBContext> options) : base (options) { }
    public virtual void Commit ()
    {
      base.SaveChanges ();
    }
    protected override void OnModelCreating (ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<FotoUpload> (entity =>
      {
        entity.ToTable ("foto_upload");

        entity.Property (e => e.Id)
          .HasColumnName ("id")
          .HasColumnType ("int(11)");

        entity.Property (e => e.FotoName)
          .IsRequired ()
          .HasColumnName ("foto_name")
          .HasMaxLength (50);

        entity.Property (e => e.FotoPath)
          .IsRequired ()
          .HasColumnName ("foto_path")
          .HasMaxLength (255);

        entity.Property (e => e.ProductId)
          .HasColumnName ("product_id")
          .HasColumnType ("int(11)");
      });

      modelBuilder.Entity<Interaction> (entity =>
      {
        entity.ToTable ("interaction");

        entity.Property (e => e.Id)
          .HasColumnName ("id")
          .HasColumnType ("int(11)");

        entity.Property (e => e.ChatBy)
          .HasColumnName ("chat_by")
          .HasColumnType ("int(11)");

        entity.Property (e => e.ChatDate)
          .HasColumnName ("chat_date")
          .HasColumnType ("datetime");

        entity.Property (e => e.Comment)
          .HasColumnName ("comment")
          .HasMaxLength (255);

        entity.Property (e => e.CommentBy)
          .HasColumnName ("comment_by")
          .HasColumnType ("int(11)");

        entity.Property (e => e.CommentDate)
          .HasColumnName ("comment_date")
          .HasColumnType ("datetime");

        entity.Property (e => e.IsChat)
          .HasColumnName ("is_chat")
          .HasColumnType ("tinyint(1)");

        entity.Property (e => e.IsComment)
          .HasColumnName ("is_comment")
          .HasColumnType ("tinyint(1)");

        entity.Property (e => e.IsLike)
          .HasColumnName ("is_like")
          .HasColumnType ("tinyint(1)");

        entity.Property (e => e.IsViewed)
          .HasColumnName ("is_viewed")
          .HasColumnType ("tinyint(1)");

        entity.Property (e => e.LikedBy)
          .HasColumnName ("liked_by")
          .HasColumnType ("int(11)");

        entity.Property (e => e.LikedDate)
          .HasColumnName ("liked_date")
          .HasColumnType ("datetime");

        entity.Property (e => e.ProductId)
          .HasColumnName ("product_id")
          .HasColumnType ("int(11)");

        entity.Property (e => e.ViewedBy)
          .HasColumnName ("viewed_by")
          .HasColumnType ("int(11)");

        entity.Property (e => e.ViewedDate)
          .HasColumnName ("viewed_date")
          .HasColumnType ("datetime");
      });

      modelBuilder.Entity<MCategory> (entity =>
      {
        entity.ToTable ("m_category");

        entity.Property (e => e.Id)
          .HasColumnName ("id")
          .HasColumnType ("int(11)");

        entity.Property (e => e.Category)
          .IsRequired ()
          .HasColumnName ("category")
          .HasMaxLength (100);
      });

      modelBuilder.Entity<MCompany> (entity =>
      {
        entity.ToTable ("m_company");

        entity.Property (e => e.Id)
          .HasColumnName ("id")
          .HasColumnType ("int(11)");

        entity.Property (e => e.Address)
          .IsRequired ()
          .HasColumnName ("address")
          .HasMaxLength (255);

        entity.Property (e => e.AddressKoordinat)
          .IsRequired ()
          .HasColumnName ("address_koordinat")
          .HasMaxLength (255);

        entity.Property (e => e.CompanyName)
          .IsRequired ()
          .HasColumnName ("company_name")
          .HasMaxLength (100);

        entity.Property (e => e.FixedCall)
          .IsRequired ()
          .HasColumnName ("fixed_call")
          .HasMaxLength (50);

        entity.Property (e => e.HoldingId)
          .HasColumnName ("holding_id")
          .HasColumnType ("int(11)");

        entity.Property (e => e.Image)
          .HasColumnName ("image")
          .HasMaxLength (255);
      });

      modelBuilder.Entity<MHolding> (entity =>
      {
        entity.ToTable ("m_holding");

        entity.Property (e => e.Id)
          .HasColumnName ("id")
          .HasColumnType ("int(11)");

        entity.Property (e => e.Address)
          .IsRequired ()
          .HasColumnName ("address")
          .HasMaxLength (255);

        entity.Property (e => e.AddressKoordinat)
          .IsRequired ()
          .HasColumnName ("address_koordinat")
          .HasMaxLength (255);

        entity.Property (e => e.FixedCall)
          .IsRequired ()
          .HasColumnName ("fixed_call")
          .HasMaxLength (50);

        entity.Property (e => e.HoldingName)
          .IsRequired ()
          .HasColumnName ("holding_name")
          .HasMaxLength (100);
      });

      modelBuilder.Entity<Notification> (entity =>
      {
        entity.ToTable ("notification");

        entity.Property (e => e.Id)
          .HasColumnName ("id")
          .HasColumnType ("int(11)");

        entity.Property (e => e.Title)
          .IsRequired ()
          .HasColumnName ("title")
          .HasMaxLength (100);

        entity.Property (e => e.Content)
          .IsRequired ()
          .HasColumnName ("content")
          .HasMaxLength (255);

        entity.Property (e => e.Topic)
          .HasColumnName ("topic")
          .HasMaxLength (100);

        entity.Property (e => e.To)
          .HasColumnName ("to")
          .HasColumnType ("int(11)");

        entity.Property (e => e.PushDate)
          .IsRequired ()
          .HasColumnName ("push_date")
          .HasColumnType ("datetime");
      });

      modelBuilder.Entity<MTypeId> (entity =>
      {
        entity.ToTable ("m_type_id");

        entity.Property (e => e.Id)
          .HasColumnName ("id")
          .HasColumnType ("int(11)");

        entity.Property (e => e.DescType)
          .IsRequired ()
          .HasColumnName ("desc_type")
          .HasMaxLength (50);
      });

      modelBuilder.Entity<MProvinsi> (entity =>
      {
        entity.ToTable ("m_provinsi");

        entity.Property (e => e.Id)
          .HasColumnName ("provinsi_id")
          .HasColumnType ("int(11)");

        entity.Property (e => e.Name)
          .IsRequired ()
          .HasColumnName ("provinsi_name")
          .HasMaxLength (255);
      });

      modelBuilder.Entity<MKabKota> (entity =>
      {
        entity.ToTable ("m_kab_kota");

        entity.Property (e => e.Id)
          .HasColumnName ("kab_kota_id")
          .HasColumnType ("int(11)");

        entity.Property (e => e.ProvinsiId)
          .IsRequired ()
          .HasColumnName ("provinsi_id")
          .HasColumnType ("int(11)");

        entity.Property (e => e.Name)
          .IsRequired ()
          .HasColumnName ("kab_kota_name");
      });

      modelBuilder.Entity<MUser> (entity =>
      {
        entity.ToTable ("m_user");

        entity.Property (e => e.Id)
          .HasColumnName ("id")
          .HasColumnType ("int(11)");

        entity.Property (e => e.Address)
          .IsRequired ()
          .HasColumnName ("address")
          .HasMaxLength (255);

        entity.Property (e => e.AddressKoordinat)
          .HasColumnName ("address_koordinat")
          .HasMaxLength (255);

        entity.Property (e => e.CompanyId)
          .HasColumnName ("company_id")
          .HasColumnType ("int(11)");

        entity.Property (e => e.Email)
          .IsRequired ()
          .HasColumnName ("email")
          .HasMaxLength (100);

        entity.Property (e => e.Handphone)
          .IsRequired ()
          .HasColumnName ("handphone")
          .HasMaxLength (15);

        entity.Property (e => e.IdNumber)
          .IsRequired ()
          .HasColumnName ("id_number")
          .HasMaxLength (50);

        entity.Property (e => e.IdType)
          .HasColumnName ("id_type")
          .HasColumnType ("int(11)");

        entity.Property (e => e.JobTitle)
          .HasColumnName ("job_title")
          .HasMaxLength (100);

        entity.Property (e => e.Name)
          .IsRequired ()
          .HasColumnName ("name")
          .HasMaxLength (100);

        entity.Property (e => e.Occupation)
          .HasColumnName ("occupation")
          .HasMaxLength (100);

        entity.Property (e => e.Password)
          .IsRequired ()
          .HasColumnName ("password")
          .HasMaxLength (255);

        entity.Property (e => e.PushId)
          .HasColumnName ("push_id")
          .HasMaxLength (255);

        entity.Property (e => e.Image)
          .HasColumnName ("image")
          .HasMaxLength (255);

        entity.Property (e => e.Username)
          .IsRequired ()
          .HasColumnName ("username")
          .HasMaxLength (50);
      });

      modelBuilder.Entity<Product> (entity =>
      {
        entity.ToTable ("product");

        entity.Property (e => e.Id)
          .HasColumnName ("id")
          .HasColumnType ("int(11)");

        entity.Property (e => e.CategoryId)
          .HasColumnName ("category_id")
          .HasColumnType ("int(11)");

        entity.Property (e => e.CompanyId)
          .HasColumnName ("company_id")
          .HasColumnType ("int(11)");

        entity.Property (e => e.Credentials)
          .HasColumnName ("credentials")
          .HasMaxLength (255);

        entity.Property (e => e.Description)
          .IsRequired ()
          .HasColumnName ("description")
          .HasMaxLength (255);

        entity.Property (e => e.HoldingId)
          .HasColumnName ("holding_id")
          .HasColumnType ("int(11)");

        entity.Property (e => e.IsIncludePrice)
          .HasColumnName ("is_include_price")
          .HasColumnType ("tinyint(1)");

        entity.Property (e => e.Currency)
          .HasColumnName ("currency")
          .HasMaxLength (3);

        entity.Property (e => e.Price).HasColumnName ("price");

        entity.Property (e => e.ProductName)
          .IsRequired ()
          .HasColumnName ("product_name")
          .HasMaxLength (200);

        entity.Property (e => e.UserId)
          .HasColumnName ("user_id")
          .HasColumnType ("int(11)");

        entity.Property (e => e.VideoPath)
          .HasColumnName ("video_path")
          .HasMaxLength (255);
      });

      modelBuilder.Entity<SysParam> (entity =>
      {
        entity.ToTable ("sys_param");

        entity.Property (e => e.Id)
          .HasColumnName ("id")
          .HasColumnType ("int(11)");

        entity.Property (e => e.Description)
          .IsRequired ()
          .HasColumnName ("description")
          .HasMaxLength (255);

        entity.Property (e => e.ParamCode)
          .IsRequired ()
          .HasColumnName ("param_code")
          .HasMaxLength (100);

        entity.Property (e => e.ParamValue)
          .IsRequired ()
          .HasColumnName ("param_value")
          .HasMaxLength (255);
      });
    }
  }
}