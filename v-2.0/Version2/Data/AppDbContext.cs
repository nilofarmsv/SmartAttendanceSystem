using Microsoft.EntityFrameworkCore;

namespace WorkSanse.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<LeaveRequest> LeaveRequest { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<TelegramBotSettings> TelegramBotSettings { get; set; }
        public DbSet<TelegramUsers> TelegramUsers { get; set; }
        public DbSet<User> User { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=.\SEPIDYARHESAB;Database=WorkSanse;User Id=damavand;Password=damavand;Trusted_Connection=True;Encrypt=False;");
        }
    }

    public partial class Attendance
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid CompanyId { get; set; }
        public DateTime Date { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public string? Text { get; set; }
    }
    public partial class Company
    {
        //public Company()
        //{
        //    Attendances = new HashSet<Attendance>();
        //}

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Phone { get; set; }
        public DateTime? CreateAt { get; set; }
    }
    public partial class Employee
    {
        //public Employee()
        //{
        //    Attendances = new HashSet<Attendance>();
        //}

        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public long? TelegramId { get; set; }
        public string Name { get; set; } = null!;
        public string Family { get; set; } = null!;
        public string NationalCode { get; set; } = null!;
        public string? Phone { get; set; }
        public string UserName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Position { get; set; } = null!;
        public byte[] filePath { get; set; }
        public DateTime? HireDate { get; set; }
        public bool? IsActive { get; set; }
    }
    public partial class LeaveRequest
    {
        public Guid Id { get; set; }
        public Guid EmploeeId { get; set; }
        public Guid CompanyId { get; set; }
        public DateTime FormDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Status { get; set; } = null!;
        public string Text { get; set; }
        public DateTime RequestAt { get; set; }
        public DateTime ReviewDate { get; set; }
    }
    public partial class Notifications
    {
        //public Notifications()
        //{
        //    Attendances = new HashSet<Attendance>();
        //}

        public Guid Id { get; set; }
        public Guid EmploeeId { get; set; }
        public string Type { get; set; } = null!;
        public string? Message { get; set; }
        public DateTime? SentAt { get; set; }
        public bool? IsSent { get; set; }

    }
    public partial class Role
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public bool Isok { get; set; }
    }
    public partial class TelegramBotSettings
    {
        //public TelegramBotSettings()
        //{
        //    Attendances = new HashSet<Attendance>();
        //}

        public Guid Id { get; set; }
        public string BotToken { get; set; }
        public string BotUserName { get; set; } = null!;
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

    }
    public partial class TelegramUsers
    {
        //public TelegramUsers()
        //{
        //    Attendances = new HashSet<Attendance>();
        //}

        public Guid Id { get; set; }
        public Guid EmploeeId { get; set; }
        public long ChatId { get; set; }
        public string UserName { get; set; } = null!;
        public string? Name { get; set; }
        public DateTime? LastInteraction { get; set; }
        public bool IsVerified { get; set; }
    }
    public partial class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; }
    }

}
