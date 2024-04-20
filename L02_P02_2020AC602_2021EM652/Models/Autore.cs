using System;
using System.Collections.Generic;

namespace L02_P02_2020AC602_2021EM652.Models;

public partial class Autore
{
    public int Id { get; set; }

    public string? Autor { get; set; }

    public virtual ICollection<Libro> Libros { get; set; } = new List<Libro>();
}
