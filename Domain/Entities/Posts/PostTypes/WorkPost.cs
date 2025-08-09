using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums.Posts;

namespace Domain.Entities.Posts.PostTypes;

public class WorkPost : Post
{
    public WorkloadType Workload { get; set; } = WorkloadType.Other;
    public WorkLocationType WorkLocation { get; set; } = WorkLocationType.OnSite;
    public ContractType Contract { get; set; } = ContractType.EmploymentContract;

    [Column(TypeName = "decimal(18,2)")] public decimal? MinSalary { get; set; }

    [Column(TypeName = "decimal(18,2)")] public decimal? MaxSalary { get; set; }
    public string? BuildingNumber { get; set; }
    public bool ExperienceRequired { get; set; } = false;
}