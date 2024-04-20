using System;
using System.Collections.Generic;

namespace L02_P02_2020AC602_2021EM652.Models;

public partial class Categoria
{
    public int Id { get; set; }

    public string? Categoria1 { get; set; }

    public virtual ICollection<Libro> Libros { get; set; } = new List<Libro>();
}
