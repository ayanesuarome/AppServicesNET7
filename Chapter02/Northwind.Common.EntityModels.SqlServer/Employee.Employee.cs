﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Packt.Shared;

public partial class Employee : IHasLastRefreshed
{
    [NotMapped]
    public DateTimeOffset LastRefreshed { get; set; }
}
