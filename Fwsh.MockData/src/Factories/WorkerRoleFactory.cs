namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;
using Fwsh.Common;

public class WorkerRoleFactory : Factory<ICollection<WorkerRole>>
{
    Random random = new Random();
    
    static string[] RoleNames = new[] {
        Roles.Carpentry,
        Roles.Sewing,
        Roles.Assembly,
        Roles.Assembly,
        Roles.Upholstery,
        Roles.Upholstery
    };

    public override ICollection<WorkerRole> Next()
    {
        int roleCount = random.Next(1, 3);
        var result = new HashSet<string>();
        
        for (int i=0; i<roleCount; i++) {
            result.Add(random.Choice(RoleNames));
        }

        return result
            .Select(role => new WorkerRole { 
                RoleName = role 
            })
            .ToList();
    }
}
