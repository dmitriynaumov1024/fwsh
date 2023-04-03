namespace Fwsh.WebApi.Results;

using System;
using System.Collections.Generic;
using System.Linq;
using Fwsh.Common;

public class TaskPrototypeResult : Result, IResultBuilder<TaskPrototypeResult>
{
    private ResultBuildingContext rbcontext;
    private TaskPrototype task;

    public int Id { get; set; }
    public int DesignId { get; set; }
    public string DesignType { get; set; }
    public string DesignName { get; set; }
    public int Precedence { get; set; }
    public string RoleName { get; set; }
    public int Payment { get; set; }
    public string Description { get; set; }
    public string InstructionUrl { get; set; }

    public DesignResult Design { get; set; }

    public List<TaskPart> Parts { get; set; }
    public List<TaskMaterial> Materials { get; set; }
    public List<TaskFabric> Fabrics { get; set; }

    public TaskPrototypeResult() { }

    public TaskPrototypeResult (TaskPrototype task, ResultBuildingContext rbcontext = null) 
    {
        this.rbcontext = rbcontext ?? new ResultBuildingContext();
        this.task = task;
    }

    public TaskPrototypeResult Mini()
    {
        return new TaskPrototypeResult() {
            Id = task.Id,
            DesignId = task.DesignId,
            DesignType = task.Design?.Type,
            DesignName = task.Design?.DisplayName,
            Precedence = task.Precedence,
            RoleName = task.RoleName,
            Payment = task.Payment,
            Description = task.Description
        };
    }

    public TaskPrototypeResult ForCustomer() 
    {
        return null;
    }

    public TaskPrototypeResult ForWorker()
    {
        var result = new TaskPrototypeResult() {
            Id = task.Id,
            DesignId = task.DesignId,
            DesignType = task.Design?.Type,
            DesignName = task.Design?.DisplayName,
            Precedence = task.Precedence,
            RoleName = task.RoleName,
            Payment = task.Payment,
            Description = task.Description,
            InstructionUrl = task.InstructionUrl,
            Parts = task.Parts.ToList(),
            Materials = task.Materials.ToList(),
            Fabrics = task.Fabrics.ToList()
        };

        if (task.Design != null && !rbcontext.Contains(result.Design)) {
            rbcontext.Add(result.Design);
            result.Design = new DesignResult(task.Design).ForWorker();
        }

        return result;
    }

    public TaskPrototypeResult ForManager()
    {
        return ForWorker();
    }

}