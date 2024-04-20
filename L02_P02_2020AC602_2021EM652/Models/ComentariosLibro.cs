using System;
using System.Collections.Generic;

namespace L02_P02_2020AC602_2021EM652.Models;

public partial class ComentariosLibro
{
    public int Id { get; set; }

    public int? IdLibro { get; set; }

    public string? Comentarios { get; set; }

    public string? Usuario { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Libro? IdLibroNavigation { get; set; }
}
