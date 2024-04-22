namespace L02_P02_2020AC602_2021EM652.Models
{
    public class CierreViewModel
    {
        public Cliente? Cliente { get; set; }

        public IEnumerable<PedidoDetalle>? PedidoDetalles { get; set; }

        public  decimal PrecioTotal { get; set; }
    }
}
