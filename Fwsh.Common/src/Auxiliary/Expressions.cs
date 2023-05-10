namespace Fwsh.Common;

using System;
using System.Linq;
using System.Linq.Expressions;

public static class E 
{
    public class Worker 
    {
        public static Expression<Func<Fwsh.Common.Worker, bool>> IsManager = 
            (worker) => worker.RolesString.Contains(WorkerRoles.Management);

        public static Expression<Func<Fwsh.Common.Worker, bool>> HasRole (string rolename) =>
            (worker) => worker.RolesString.Contains(rolename);
    }
}
