using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mini_ifms.Models.Entities;

public class Project
{
    [Key]
    public int ProjectId { get; set; }

    [Required, MaxLength(30)]
    public string ProjectCode { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string ProjectName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Package> Packages { get; set; } = new List<Package>();
    public ICollection<HolidayCalendar> Holidays { get; set; } = new List<HolidayCalendar>();
}

public class Package
{
    [Key]
    public int PackageId { get; set; }

    public int ProjectId { get; set; }

    [Required, MaxLength(20)]
    public string PackageCode { get; set; } = string.Empty; // e.g., GEP, SURF, OLNG, FPSO

    [Required, MaxLength(150)]
    public string PackageName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; } = null!;

    public ICollection<UserPackage> UserPackages { get; set; } = new List<UserPackage>();
}

public class Discipline
{
    [Key]
    public int DisciplineId { get; set; }

    [Required, MaxLength(20)]
    public string DisciplineCode { get; set; } = string.Empty; // e.g., PROC, MECH, ELEC, INST, PIPE

    [Required, MaxLength(100)]
    public string DisciplineName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required, MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(30)]
    public string GlobalRole { get; set; } = "User"; // Admin, User

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<UserPackage> UserPackages { get; set; } = new List<UserPackage>();
}

public class UserPackage
{
    [Key]
    public int UserPackageId { get; set; }

    public int UserId { get; set; }
    public int PackageId { get; set; }

    [Required, MaxLength(40)]
    public string PackageRole { get; set; } = string.Empty; // Initiator, PackageCoordinator, PIC, Reviewer

    public bool IsActive { get; set; } = true;
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [ForeignKey(nameof(PackageId))]
    public Package Package { get; set; } = null!;
}

public class HolidayCalendar
{
    [Key]
    public int HolidayId { get; set; }

    public int? ProjectId { get; set; } // Null = global/national holiday

    [Column(TypeName = "date")]
    public DateTime HolidayDate { get; set; }

    [Required, MaxLength(150)]
    public string Description { get; set; } = string.Empty;

    [ForeignKey(nameof(ProjectId))]
    public Project? Project { get; set; }
}
