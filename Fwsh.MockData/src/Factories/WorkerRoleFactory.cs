namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;
using Fwsh.Common;

public class WorkerRoleFactory : Factory<List<string>>
{
    Random random = new Random();
    
    static string[] RoleNames = new[] {
        WorkerRoles.Carpentry,
        WorkerRoles.Sewing,
        WorkerRoles.Assembly,
        WorkerRoles.Assembly,
        WorkerRoles.Upholstery,
        WorkerRoles.Upholstery
    };

    public override List<string> Next()
    {
        int roleCount = random.Next(1, 3);
        var result = new HashSet<string>();
        
        for (int i=0; i<roleCount; i++) 
            result.Add(random.Choice(RoleNames));
        
        return result.ToList();
    }
}
