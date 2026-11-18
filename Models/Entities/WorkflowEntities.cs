using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mini_ifms.Models.Entities;

public class WorkflowDefinition
{
    [Key]
    public int WorkflowDefId { get; set; }

    [Required, MaxLength(50)]
    public string WorkflowCode { get; set; } = string.Empty; // e.g., IFQ_STANDARD

    [Required, MaxLength(150)]
    public string WorkflowName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<WorkflowStep> Steps { get; set; } = new List<WorkflowStep>();
    public ICollection<WorkflowTransition> Transitions { get; set; } = new List<WorkflowTransition>();
}

public class WorkflowStep
{
    [Key]
    public int WorkflowStepId { get; set; }

    public int WorkflowDefId { get; set; }

    [Required, MaxLength(40)]
    public string StepCode { get; set; } = string.Empty; // e.g., DRAFT, SUBMITTED, ASSIGNED, RESPONDED, CLOSED

    [Required, MaxLength(100)]
    public string StepName { get; set; } = string.Empty;

    public int StepOrder { get; set; }

    [MaxLength(40)]
    public string? RequiredRole { get; set; } // PackageRole required to act on this step

    public int SlaDays { get; set; } = 0; // Working days SLA for this step
    public bool IsInitial { get; set; }
    public bool IsFinal { get; set; }

    [ForeignKey(nameof(WorkflowDefId))]
    public WorkflowDefinition WorkflowDefinition { get; set; } = null!;
}

public class WorkflowTransition
{
    [Key]
    public int TransitionId { get; set; }

    public int WorkflowDefId { get; set; }
    public int FromStepId { get; set; }
    public int ToStepId { get; set; }

    [Required, MaxLength(50)]
    public string ActionName { get; set; } = string.Empty; // Submit, Assign, Respond, RequestClarification, Reopen, Close

    [Required, MaxLength(40)]
    public string RequiredRole { get; set; } = string.Empty;

    public bool RequiresComment { get; set; }

    [ForeignKey(nameof(WorkflowDefId))]
    public WorkflowDefinition WorkflowDefinition { get; set; } = null!;

    [ForeignKey(nameof(FromStepId))]
    public WorkflowStep FromStep { get; set; } = null!;

    [ForeignKey(nameof(ToStepId))]
    public WorkflowStep ToStep { get; set; } = null!;
}

public class WorkflowInstance
{
    [Key]
    public long WorkflowInstanceId { get; set; }

    public int WorkflowDefId { get; set; }

    [Required, MaxLength(50)]
    public string EntityType { get; set; } = "InterfaceQuery";

    public long EntityId { get; set; }
    public int CurrentStepId { get; set; }

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    [ForeignKey(nameof(WorkflowDefId))]
    public WorkflowDefinition WorkflowDefinition { get; set; } = null!;

    [ForeignKey(nameof(CurrentStepId))]
    public WorkflowStep CurrentStep { get; set; } = null!;

    public ICollection<WorkflowApproval> Approvals { get; set; } = new List<WorkflowApproval>();
}

public class WorkflowApproval
{
    [Key]
    public long ApprovalId { get; set; }

    public long WorkflowInstanceId { get; set; }
    public int? FromStepId { get; set; }
    public int ToStepId { get; set; }

    [Required, MaxLength(50)]
    public string ActionName { get; set; } = string.Empty;

    public int ActionByUserId { get; set; }
    public DateTime ActionAt { get; set; } = DateTime.UtcNow;

    [MaxLength(1000)]
    public string? Remarks { get; set; }

    [ForeignKey(nameof(WorkflowInstanceId))]
    public WorkflowInstance WorkflowInstance { get; set; } = null!;

    [ForeignKey(nameof(FromStepId))]
    public WorkflowStep? FromStep { get; set; }

    [ForeignKey(nameof(ToStepId))]
    public WorkflowStep ToStep { get; set; } = null!;

    [ForeignKey(nameof(ActionByUserId))]
    public User ActionByUser { get; set; } = null!;
}
