using System.Configuration;
using System.Data.Entity;
using System.Web;
using NS.Entity;
using PdfSharp.Drawing;
using XProject.Domain.Entities;

namespace XProject.Domain
{
    public class XProjectDb : DbContext
    {
        public XProjectDb()
        {
            
            // hook up the Migrations configuration
            Database.SetInitializer<XProjectDb>(null);
            //Configuration.ProxyCreationEnabled = false;
            //Configuration.LazyLoadingEnabled = false;
        }

        //public KienTrucDb(string connectionName)
        //    : base("name=" + connectionName)
        //{
        //}
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<Menu> MenuItems { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Setting> Settings { get; set; }

        //public DbSet<XState> States { get; set; }
        //public DbSet<XStateText> StateTexts { get; set; }
        //public DbSet<XDistrict> Districts { get; set; }
        //public DbSet<XDistrictText> DistrictTexts { get; set; }
        //public DbSet<XLocation> Locations { get; set; }
        //public DbSet<XLocationText> LocationTexts { get; set; }


      



        public DbSet<XCountry> XCountries { get; set; }
        public DbSet<XPicture> XPictures { get; set; }
        public DbSet<XMenu> XMenus { get; set; }
        public DbSet<XSlider> XSliders { get; set; }
        public DbSet<XNew> XNews { get; set; }
        public DbSet<XContent> XContents { get; set; }
        public DbSet<XEmail> XEmails { get; set; }
        public DbSet<XContentDetail> XContentDetails { get; set; }
        public DbSet<XProduct> XProducts { get; set; }
        
        public DbSet<XType> XTypes { get; set; }
        public DbSet<XAttribute> XAttributes { get; set; }
        public DbSet<XPartner> XPartners { get; set; }
        public DbSet<XSeo> XSEOs { get; set; }



        protected override void OnModelCreating(DbModelBuilder mb)
        {

            mb.Entity<UserLogin>().HasMany(u => u.Roles).WithMany().Map(map => map.ToTable("UserLogins_Roles").MapLeftKey("UserLogin_Id").MapRightKey("Role_Id"));
            mb.Entity<Role>().HasMany(r => r.Permissions).WithMany().Map(map => map.ToTable("Roles_Permissions").MapLeftKey("Role_Id").MapRightKey("Permission_Id"));
            mb.Entity<Menu>().HasMany(m => m.Roles).WithMany().Map(map => map.ToTable("Menus_Roles").MapLeftKey("Menu_Id").MapRightKey("Role_Id"));
        }
    }
}
