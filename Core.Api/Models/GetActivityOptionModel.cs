

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models
{
    
    

public class GetActivityOptionModel
    {
    public int? option_id { get; set; }
    public int? fromAge { get; set; }
    public int? toAge { get; set; }
    public string _class { get; set; }
    public string name { get; set; }
}
}
