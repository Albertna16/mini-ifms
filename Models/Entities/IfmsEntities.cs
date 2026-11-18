using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mini_ifms.Models.Entities;

public class InterfaceQuery
{
    [Key]
    public long QueryId { get; set; }

    [Required, MaxLength(40)]
    public string QueryNumber { get; set; } = string.Empty; // e.g., DRAFT-GUID or IFQ-GEP-SURF-0001

    public int ProjectId { get; set; }
    public int FromPackageId { get; set; }
    public int ToPackageId { get; set; }
    public int DisciplineId { get; set; }

    [Required, MaxLength(250)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Priority { get; set; } = "Medium"; // Low, Medium, High, Critical

    public long? WorkflowInstanceId { get; set; }
    public int CurrentStepId { get; set; }

    public int CreatedByUserId { get; set; }
    public int? AssignedPicUserId { get; set; }

    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? SubmittedAt { get; set; }
    public DateTime? ClosedAt { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; } = null!;

    [ForeignKey(nameof(FromPackageId))]
    public Package FromPackage { get; set; } = null!;

    [ForeignKey(nameof(ToPackageId))]
    public Package ToPackage { get; set; } = null!;

    [ForeignKey(nameof(DisciplineId))]
    public Discipline Discipline { get; set; } = null!;

    [ForeignKey(nameof(WorkflowInstanceId))]
    public WorkflowInstance? WorkflowInstance { get; set; }

    [ForeignKey(nameof(CurrentStepId))]
    public WorkflowStep CurrentStep { get; set; } = null!;

    [ForeignKey(nameof(CreatedByUserId))]
    public User CreatedByUser { get; set; } = null!;

    [ForeignKey(nameof(AssignedPicUserId))]
    public User? AssignedPicUser { get; set; }

    public ICollection<QueryResponse> Responses { get; set; } = new List<QueryResponse>();
    public ICollection<Document> Documents { get; set; } = new List<Document>();
}

public class QueryResponse
{
    [Key]
    public long ResponseId { get; set; }

    public long QueryId { get; set; }
    public int RespondedByUserId { get; set; }

    [Required]
    public string ResponseText { get; set; } = string.Empty;

    [Required, MaxLength(30)]
    public string ResponseType { get; set; } = "Investigation"; // Investigation, Clarification, ReviewNote

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(QueryId))]
    public InterfaceQuery Query { get; set; } = null!;

    [ForeignKey(nameof(RespondedByUserId))]
    public User RespondedByUser { get; set; } = null!;
}

public class QuerySequence
{
    [Key]
    public int SequenceId { get; set; }

    public int FromPackageId { get; set; }
    public int ToPackageId { get; set; }
    public int LastNumber { get; set; }

    [ForeignKey(nameof(FromPackageId))]
    public Package FromPackage { get; set; } = null!;

    [ForeignKey(nameof(ToPackageId))]
    public Package ToPackage { get; set; } = null!;
}

public class Document
{
    [Key]
    public long DocumentId { get; set; }

    public long QueryId { get; set; }

    [Required, MaxLength(200)]
    public string DocumentTitle { get; set; } = string.Empty;

    public int CurrentRevisionNo { get; set; } = 0;
    public int CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(QueryId))]
    public InterfaceQuery Query { get; set; } = null!;

    [ForeignKey(nameof(CreatedByUserId))]
    public User CreatedByUser { get; set; } = null!;

    public ICollection<DocumentRevision> Revisions { get; set; } = new List<DocumentRevision>();
}

public class DocumentRevision
{
    [Key]
    public long RevisionId { get; set; }

    public long DocumentId { get; set; }
    public int RevisionNo { get; set; }

    [Required, MaxLength(255)]
    public string FileName { get; set; } = string.Empty;

    [Required, MaxLength(500)]
    public string StoragePath { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string ContentType { get; set; } = string.Empty;

    public long FileSizeBytes { get; set; }

    [MaxLength(128)]
    public string? FileHash { get; set; }

    [MaxLength(500)]
    public string? ChangeNotes { get; set; }

    public int UploadedByUserId { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    public bool IsCurrent { get; set; } = true;

    [ForeignKey(nameof(DocumentId))]
    public Document Document { get; set; } = null!;

    [ForeignKey(nameof(UploadedByUserId))]
    public User UploadedByUser { get; set; } = null!;
}

public class AuditLog
{
    [Key]
    public long AuditLogId { get; set; }

    [Required, MaxLength(100)]
    public string TableName { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string RecordId { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Action { get; set; } = string.Empty; // INSERT, UPDATE, DELETE, WORKFLOW

    public string? OldValues { get; set; } // JSON
    public string? NewValues { get; set; } // JSON

    public int? ChangedByUserId { get; set; }
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(64)]
    public string? CorrelationId { get; set; }

    [ForeignKey(nameof(ChangedByUserId))]
    public User? ChangedByUser { get; set; }
}
