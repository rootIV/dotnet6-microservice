﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.Api.Model.Base;

public class BaseEntitie
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
}
