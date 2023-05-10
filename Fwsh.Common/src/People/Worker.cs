namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public class Worker : Person
{
    private string _rolesString;
    private IReadOnlyCollection<string> _roles;

    public string RolesString { 
        get { 
            return _rolesString; 
        }
        set { 
            _rolesString = value; 
            _roles = (value ?? String.Empty).Split(";"); 
        }
    }

    public IReadOnlyCollection<string> Roles {
        get { 
            if (_roles == null) _roles = (_rolesString ?? String.Empty).Split(";");
            return _roles; 
        }
        set { 
            _roles = value; 
            _rolesString = String.Join(";", value); 
        }
    }

    public virtual ICollection<WorkerPaycheck> Paychecks { get; set; }

    public virtual ICollection<ProdTask> ProdTasks { get; set; }
    public virtual ICollection<RepairTask> RepairTasks { get; set; }

    public bool HasRole (string rolename) => this.RolesString.Contains(rolename);

    public bool IsManager => this.RolesString.Contains(WorkerRoles.Management);

    public Worker() : base()
    {
        this.Paychecks = new HashSet<WorkerPaycheck>();
        
        this.ProdTasks = new HashSet<ProdTask>();
        this.RepairTasks = new HashSet<RepairTask>();
    }

}
